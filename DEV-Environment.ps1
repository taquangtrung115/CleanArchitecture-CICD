pipeline {
    agent any
    
    environment {
		GIT_BRANCH = 'DEV'
		GitToken = 'dgk7r4t3wm7uznn27u6uqflwlhucczn4tbcfdbzszxrv7ijgizqq'
		GitUrl = "https://${env.GitToken}@dev.azure.com/TDSolutionArchitecture/DemoCICD/_git/DemoCICD"
		ENV = 'Development'
		BUILD_CONFIG = 'Release'
		DOTNET_VERSION = 'net7.0'
		SLN = '.\\src\\DemoCICD.API\\DemoCICD.API.csproj'
		WEB_SITE = 'sieupham.dev.com'
		APP_POOL = 'sieupham.dev.com'
		PUBLISH_PATH = '.\\src\\DemoCICD.API\\bin\\%BUILD_CONFIG%\\%DOTNET_VERSION%\\publish'
		WWW_ROOT = 'C:\\WWW\\DemoCICD\\BE\\DEV'
		
		SlnUnitTest = '.\\DemoCICD.sln'
		TestResultFileName = 'UnitTestRestult.trx'
		TrxFilePath = '.\\test\\DemoCICD.Architecture.Tests\\TestResults'
		MainDirectory = 'C:\\WWW\\DemoCICD\\TestResults\\'
	}

    stages {
        stage('Git Checkout') {
            steps {
                git branch: "${env.GIT_BRANCH}", url: "${env.GitUrl}"
            }
        }
        
        stage('Restore Nuget Package') {
            steps {
                bat "dotnet restore ${env.SLN}"
            }
        }
        
        stage('Clean') {
            steps {
                bat "dotnet clean ${env.SLN}"
            }
        }
        
        stage('Build') {
            steps {
                bat "dotnet build ${env.SLN} --configuration ${env.BUILD_CONFIG}"
            }
        }
        
        stage('Unit Test') {
            steps {
                bat "dotnet test ${env.SlnUnitTest} -l:trx;LogFileName=${env.TestResultFileName}"
                
                bat "if not exist ${env.MainDirectory} mkdir ${env.MainDirectory}"
                bat "copy ${env.TrxFilePath}\\${env.TestResultFileName} ${env.MainDirectory}"
            }
        }
        
        stage('Publish') {
            steps {
                bat "dotnet publish ${env.SLN} /p:Configuration=${env.BUILD_CONFIG} /p:EnvironmentName=${env.ENV}"
            }
        }
        
        stage('Stop IIS SERVER') {
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
