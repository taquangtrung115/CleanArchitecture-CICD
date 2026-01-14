pipeline {
    agent any
    // Cron trigger: chạy định kỳ ~5 phút một lần (H/5 phân phối load tốt hơn)
    triggers {
        cron('H/5 * * * *')
    }
    environment {
        GIT_BRANCH = 'DEV1.1'
        GitToken = 'ATBBA4yXVSuUw8PQvgDquDR2UaVWBD997693'
        GitUrl = "https://trungtq3:${env.GitToken}@bitbucket.org/haphuongmitalabvn/mt.uploadfiles.git"
        ENV = 'namphuong'
        BUILD_CONFIG = 'Release'
        DOTNET_VERSION = 'net8.0'
        SLN = '.\\source\\UploadFiles\\NhapKhau\\UploadFiles.NhapKhau.WSAPI\\UploadFiles.NhapKhau.WSAPI.csproj'
        PUBLISH_PATH = '.\\source\\UploadFiles\\NhapKhau\\UploadFiles.NhapKhau.WSAPI\\bin\\%BUILD_CONFIG%\\%DOTNET_VERSION%\\publish'
        WWW_ROOT = 'D:\\uploadfiles\\BE'
        BACKUP = 'D:\\uploadfiles\\BACKUP_BE\\BACKUP_%date:~4,2%%date:~7,2%_0%time:~1,1%%time:~3,2%%time:~6,2%'
        SERVICES_NAME = 'UploadFiles.WSAPI'
    }

    stages {
		stage('Git checkout') {
            
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
        stage('Publish') {
            steps {
                bat "dotnet publish ${env.SLN} /p:Configuration=${env.BUILD_CONFIG} /p:EnvironmentName=${env.ENV}"
            }
        }
        stage('Stop Service') {
            steps {
                bat '''
                    rem Kiểm tra tồn tại
                    sc query "%SERVICES_NAME%" >nul 2>&1
                    if errorlevel 1 (
                        echo Service "%SERVICES_NAME%" not found.
                        exit /b 1060
                    )
                    echo Stopping "%SERVICES_NAME%"...
                    sc stop "%SERVICES_NAME%"
                    rem đợi tối đa 30s cho service dừng
                    for /l %%i in (1,1,30) do (
                        sc query "%SERVICES_NAME%" | find /i "STATE" | find /i "STOPPED" >nul && goto :stopped
                        timeout /t 1 >nul
                    )
                    :stopped
                '''
            }
        }
        stage('Copy to hosted website folder') {
            steps {
                bat "xcopy ${env.PUBLISH_PATH} ${env.WWW_ROOT} /e /y /i /r"
            }
        }
        stage('Start Service') {
            steps {
                bat '''
                    echo Starting "%SERVICES_NAME%"...
                    sc start "%SERVICES_NAME%"
                '''
            }
        }
        
    }
}
