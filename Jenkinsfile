pipeline {
	agent any

	stages {
		stage('build') {
			steps {
                bat "\"C:/Program Files (x86)/NUnit.org/nunit-console/nunit3-console.exe\" restore SchiffeVersenken.Tests/packages.config"
                bat "\"${tool 'MSBuild'}\" SchiffeVersenken.sln -restore -t:Build /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"
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