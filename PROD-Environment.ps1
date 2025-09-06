pipeline {
    agent any
	
	environment {
		GIT_BRANCH = 'PROD'
		GitToken = 'dgk7r4t3wm7uznn27u6uqflwlhucczn4tbcfdbzszxrv7ijgizqq'
		GitUrl = "https://${env.GitToken}@dev.azure.com/TDSolutionArchitecture/DemoCICD/_git/DemoCICD"
		ENV = 'Production'
		BUILD_CONFIG = 'Release'
		DOTNET_VERSION = 'net7.0'
		SLN = '.\\src\\DemoCICD.API\\DemoCICD.API.csproj'
		WEB_SITE = 'sieupham.prod.com'
		APP_POOL = 'sieupham.prod.com'
		PUBLISH_PATH = '.\\src\\DemoCICD.API\\bin\\%BUILD_CONFIG%\\%DOTNET_VERSION%\\publish'
		WWW_ROOT = 'C:\\WWW\\DemoCICD\\BE\\PROD'
		
		BACKUP = 'C:\\WWW\\DemoCICD\\BE\\PROD_BACKUP\\PROD_%date:~-4%%date:~4,2%%date:~7,2%_%time:~0,2%%time:~3,2%%time:~6,2%'
	}

    stages {
       stage('Backup'){
           steps{
				// Backup file
				bat "xcopy ${env.WWW_ROOT} ${env.BACKUP} /e /y /i /r"
           }
       }
       
		stage('Cloning') {
			steps {
				// Get source code from a bitbucket repository
				git branch: "${env.GIT_BRANCH}", url: "${env.GitUrl}"
			}
		}
		
		stage('Restore packages'){
			steps{
				bat "dotnet restore ${env.SLN}"
			}
		}
		
		stage('Clean'){
			steps{
				bat "dotnet clean ${env.SLN}"
			}
		}
		
		stage('Build'){
			steps{
				bat "dotnet build ${env.SLN} --configuration ${env.BUILD_CONFIG}"
			}
		}
    
		stage('Publish'){
			steps{
				bat "dotnet publish ${env.SLN} /p:Configuration=${env.BUILD_CONFIG} /p:EnvironmentName=${env.ENV}"
			}
		}
		stage('Stop IIS SERVER service') { 
			steps {
				bat "%windir%\\system32\\inetsrv\\appcmd stop sites ${env.WEB_SITE}"
				bat "%windir%\\system32\\inetsrv\\appcmd stop apppool /apppool.name:${env.APP_POOL}"
				bat "echo waiting until service stopped"
				bat "ping google.com /n 5"
			}
		}
		
		stage('Copy to hosted website folder') { 
			steps {
				bat "xcopy ${env.PUBLISH_PATH} ${env.WWW_ROOT} /e /y /i /r"
			}
		}
		
		stage('Start service') { 
			steps {
				bat "%windir%\\system32\\inetsrv\\appcmd start apppool /apppool.name:${env.APP_POOL}"
				bat "%windir%\\system32\\inetsrv\\appcmd start sites ${env.WEB_SITE}"
			}
		}
	}
}