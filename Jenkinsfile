pipeline {
	agent any

	stages {
		stage('build') {
			steps {
				gerritReview score:-1
				bat "\"${NuGet}\" restore SchiffeVersenken.Tests/packages.config -PackagesDirectory packages"
				bat "\"${tool 'MSBuild'}\" SchiffeVersenken.sln -t:Build /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"
			}
		}
		stage('test') {
			steps {
				bat "\"${NUnit}\" SchiffeVersenken.Tests/bin/Release/SchiffeVersenken.Tests.dll --result=TestR.xml;format=nunit2"
			}
		}
	}

	post {
		success {
			gerritReview score:1
		}
		failure {
			gerritReview score:-1
		}
	}
}