pipeline {
    agent any
    environment {
        UNITY_PATH = 'D:\\Program Files\\Unity\\Hub\\Editor\\2020.3.38f1\\Editor\\Unity.exe'    // Unity editor path
        PROJECT_PATH = "D:\\UnityWork\\SDKFramework\\HABBY_CN"                                  // Project folder name
        EXPORT_METHOD = "SDKFramework.Editor.Tools.BuildUnityProject.PerformAndroidBuild"       // build func
        ANDROID_PROJECT = 'D:\\UnityWork\\SDKFramework\\HABBY_CN\\AndroidProject'               // Android project path
    }
    parameters {
        choice(name: 'BRANCH_NAME', choices: ['main', 'develop'], description: 'Choice a git branch to build.')
        booleanParam(name: 'ENABLE_DEBUG', defaultValue: true, description: 'Enable or disable debug features.')
        booleanParam(name: 'DEV_BUILD', defaultValue: false, description: 'Enable or disable development build features.')
        string(name: 'VERSION_NAME', defaultValue: '1.0.0', description: 'Set version name for the build.')
        string(name: 'VERSION_CODE', defaultValue: '10', description: 'Set version code for the build.')
    }
    stages {
        stage('Clean Build Cache') {
            steps {
                //  bat "rmdir /S /Q %ANDROID_PROJECT%"
                //  bat "mkdir %ANDROID_PROJECT%"
                echo '---------自测，不删除构建缓存------------'
            }
        }
        stage('Checkout') {
            steps {
                // 使用适用于您的 Unity 项目存储库的适当 SCM 检出实现 (例如 Git、SVN)
                git branch: "${params.BRANCH_NAME}", url: 'https://github.com/LinWansha/SDKFramework.git' // 设置正确的存储库 URL
            }
        }
        stage('Export from Unity') {
            steps {
                catchError {  //buildResult: null, stageResult: 'FAILURE'
                    bat "\"${UNITY_PATH}\" -batchmode -nographics -quit -projectPath \"${PROJECT_PATH}\" -executeMethod ${EXPORT_METHOD} -enableDebug=${params.ENABLE_DEBUG} -dev=${params.DEV_BUILD} -VersionName=${params.VERSION_NAME} -VersionCode=${params.VERSION_CODE} -logFile"
                }
            }
        }
        stage('Setup Gradle Wrapper') {
            steps {
                script {
                    dir("${ANDROID_PROJECT}") {
                        bat 'gradle wrapper'
                    }
                }
            }
        }
        stage('Build APK with Gradle') {
            steps {
                script {
                    if (params.ENABLE_DEBUG) {
                        dir("${ANDROID_PROJECT}") {
                            bat '.\\gradlew launcher:assembleDebug'
                        }
                    } 
                    else {
                        dir("${ANDROID_PROJECT}") {
                            bat '.\\gradlew launcher:assembleRelease'
                        }
                    }
                }
            }
        }
        stage('Copy APK to workspace & Rename apk') {
            steps {
                script {
                    // 设置游戏名称、版本名称和版本代码
                    def gameName = 'archero'
                    def brachName = params.BRANCH_NAME
                    def versionName = params.VERSION_NAME
                    def versionCode = params.VERSION_CODE
                    def buildType = params.ENABLE_DEBUG ? 'debug' : 'release'

                    def originalPath = "D:\\UnityWork\\SDKFramework\\HABBY_CN\\AndroidProject\\launcher\\build\\outputs\\apk\\${buildType}\\*-${buildType}.apk"
                    bat "copy \"${originalPath}\" \"%cd%\""


                    // 重命名 APK 文件
                    bat "for %%i in (*-debug.apk) do move /Y \"%%i\" \"%cd%\\${gameName}_${buildType}_${brachName}_${versionName}.${versionCode}.apk\""
                }
            }
        }
        stage('Archive APK') {
            steps {
                archiveArtifacts artifacts: "**/*.apk", fingerprint: true, onlyIfSuccessful: true
                echo '---------------------------------------------'
                echo 'Download the APK file from the\nArtifacts section of this build on the Jenkins web interface.'
                echo '---------------------------------------------'

                // 删除 Workspace 下所有的 .apk 文件
                bat "for /r \"%cd%\" %%f in (*.apk) do del /F /Q \"%%f\""
            }
        }
    }
}