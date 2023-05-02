# FireTruckApp

Our app is designed to provide firefighters with a comprehensive tool for accessing critical information about the tooling present in their fire trucks. Our goal is to streamline the process of learning about and utilizing the equipment available, with the aim of improving operational effectiveness and safety. By creating a centralized repository of up-to-date information, we aim to empower firefighters to make informed decisions quickly, ultimately contributing to the protection of lives and property.

## Table of Contents

- [Overview](#overview)
- [Batches](#batches)
- [Installation](#installation)
- [Usage](#usage)
- [API Reference](#api-reference)
- [Contributing](#contributing)
- [License](#license)

## Overview

Welcome to the open source project for learning about the tooling in a firetruck! This project was created by me, with the goal of sharing information about all firetrucks in my department. The aim is to create a backend application and a web or mobile app that allows users to access data via an API. To be on the same status.

The primary objective is to make the information as up-to-date as possible and easily accessible to firefighters and other personnel who work with firetrucks. Currently, the project is designed to support only one tenant.

By contributing to this project, you can help improve the tooling in firetrucks and make firefighting safer and more efficient. Feel free to try this out, or ask for a demo or help.

We welcome any contributions, whether it's bug fixes, feature suggestions, or code improvements. Let's work together to make this app the best it can be!

## Batches

![](https://api.checklyhq.com/v1/badges/checks/529b4a2c-f128-4a14-8d00-c5d00ea7c3db?style=flat&theme=default)

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-black.svg)](https://sonarcloud.io/summary/new_code?id=Konry_FireTruckApp)

[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Konry_FireTruckApp&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Konry_FireTruckApp)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Konry_FireTruckApp&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Konry_FireTruckApp)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Konry_FireTruckApp&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Konry_FireTruckApp)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Konry_FireTruckApp&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Konry_FireTruckApp)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Konry_FireTruckApp&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Konry_FireTruckApp)

[![Build Status](https://konry.visualstudio.com/FireTruckApp/_apis/build/status%2FKonry.FireTruckApp?branchName=develop)](https://konry.visualstudio.com/FireTruckApp/_build/latest?definitionId=2&branchName=develop)
[![Build Status](https://konry.visualstudio.com/FireTruckApp/_apis/build/status%2FKonry.FireTruckApp?branchName=main)](https://konry.visualstudio.com/FireTruckApp/_build/latest?definitionId=2&branchName=main)

## Installation

* Getting a server ready which is accessible by the internet
* Getting the docker container 
    * docker pull konry11/firetruckapi:latest
* Api is accessible via port 80 or 443
* Environment variables are set:
    * [optional] TZ
    * ASPNETCORE_ENVIRONMENT

### TODO
[ ] Add docker-compose file here 
[ ] Do a dry run with everything described here

## Usage

TODO When mvp is ready, write here some down

## API Reference

TODO upcoming swagger api / postman collection

## Contributing

We welcome contributions to our project and appreciate any feedback or suggestions. If you would like to contribute, please follow these steps:

Fork the repository and clone it to your local machine.
Create a new branch for your changes: git checkout -b feature/your-feature-name
Make your changes and commit them with clear and concise commit messages.
Push your changes to your forked repository: git push -u origin feature/your-feature-name
Create a pull request against our develop branch.
Wait for a review and feedback from our team. We may ask you to make some changes or improvements.
Once your pull request is approved, we will merge it into our develop branch.
Please note that all pull requests must pass our code quality standards and must go through SonarCloud for analysis. We follow the clean code paradigm and expect all code contributions to meet our standards for readability, maintainability, and efficiency.

Thank you for your contributions!

## License

### MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
