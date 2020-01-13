pipeline {
   agent any

   stages {
     stage('build') {
        steps {
            bat "\"${tool 'MSBuild'}\" SchiffeVersenken.sln /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"
        }
     }
     stage('test') {
        steps {
            sh "echo 'TODO RUN TEST'"
        }
     }
   }
   
   post {
	success { gerritReview score:1 }
	failure { gerritReview score:-1 }
   }
}