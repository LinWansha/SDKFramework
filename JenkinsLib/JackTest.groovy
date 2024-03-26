pipeline {
    agent any

    stages {
         stage('Build and Test') {
            steps {
                script {
                    // 在这里执行实际的构建和测试命令，可能是 `mvn test` 或其他
                    
                    // 检查 REPORTS_DIR 文件夹是否存在，若不存在则创建
                    def REPORTS_DIR = 'target\\reports'
                    if (!fileExists(REPORTS_DIR)) {
                        bat "mkdir ${REPORTS_DIR}"
                    }
                    
                    // 检查 JUnit XML 报告文件是否存在，若不存在则创建一个示例文件
                    def junitXmlFile = "${REPORTS_DIR}\\TEST-example.xml"
                    if (!fileExists(junitXmlFile)) {
                        echo "JUnit XML report file not found. Generating a sample..."
                        def content = '''
                            <?xml version="1.0" encoding="UTF-8"?>
                            <testsuite name="Jenkins Sample Test Suite" tests="1" errors="0" failures="0" time="0">
                                <testcase name="sampleTest" classname="SampleClass" time="0"/>
                            </testsuite>
                        '''
                        writeFile file: junitXmlFile, text: content.trim()
                    } else {
                        echo "Found JUnit XML report file: ${junitXmlFile}"
                    }
                }
            }
        }
        stage('say hello'){
            steps{
                echo('hello world')
            }
        }
    }
    post {   
        always {
            echo 'This will always run'
             // 发现并归档 JUnit XML 报告文件
            junit '**\\target\\reports\\TEST-*.xml'
            //mail to: 'mengruiqing@habby.com',
            //subject: "Pipeline: ${currentBuild.fullDisplayName}",
            //body: "body ${env.BUILD_URL}"//Something is wrong with
             emailext (
                to: 'mengruiqing@habby.com',
                subject: "Build Result: ${currentBuild.result}",
                body: "The build '${env.JOB_NAME}#${env.BUILD_NUMBER}' has finished with status '${currentBuild.result}'"
            )
        }
        success {
            echo 'This will run only if successful'
        }
        failure {
            echo 'This will run only if failed'
        }
        unstable {
            echo 'This will run only if the run was marked as unstable'
        }
        changed {
            echo 'This will run only if the state of the Pipeline has changed'
            echo 'For example, if the Pipeline was previously failing but is now successful'
        }
    }
}