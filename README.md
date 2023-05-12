# CodeChallenge.JobProcess
Introduction
At XYZ we deal with high volumes of data to be processed for migration from a legacy core banking application to our cloud core banking platform. For this, clients are expected to use our jobs API. This API validates, processes, and returns statuses. All to enable the clients to test and perform data-migrations.

Requirements
You are asked to develop a service that will allow our clients to process their data in our infrastructure. The service must be delivered as:

a REST API to allow easy integration to other systems

and must be written in Asp .net core.

Also, it is required to have the following features for the consumers:

As a consumer I want to START A TYPE OF JOB  so that the provided data can be processed. For the moment, we support:

BULK jobs: Processes all the provided data in sequence. For this, the process should not stop if one fails. There is no rollback.

BATCH jobs: Processes all the provided data in sequence. For this, the process stops, if one fails. There is no rollback.

We expect more types of jobs in the future.

As a consumer I want to CHECK THE STATUS OF A JOB so that I can see its progress. The status of a job is defined by:

TOTAL number of items.

Number of PROCESSED items.

Number of FAILED items.

As a consumer I want to GET LOGS FROM A JOB so that I can see everything has gone as expected or check the errors. The logs of a job are the information of each job item execution:

SUCCESS of FAILURE.

DESCRIPTION of the problem.

Additional info
Another team is working on this project and they will implement the service responsible for the job data processing. They will expose to you a service that receives a single item, processes it and returns the status of the execution. They told you that, in average, it might take up to 500 milliseconds to process one single element.

Evaluation
Your teammates are going to review your work and they provided a list of things they look when doing code reviews:

MUST-HAVE:
Teammate will specifically check that your solution compiles

Proper use of the .NET core framework

Clean code and SOLID principles

There is proper use of dependency-injection and n-layer architecture

START A TYPE OF JOB feature is functionally implemented. Meaning that there is enough code to make it work properly

There is unit testing. At least one complete flow, from endpoint to repository, is tested. Either by a single component test or by several unit-tests on each class involved with proper mocking.

We won't evaluate your knowledge in specific technologies (Databases, infrastructure, logging, …). Show us you know how to reason, prioritize and execute. Think carefully on what it is asked in the assignment and what it is not.

SHOULD-HAVE:
Do you consider yourself an experienced developer? Then, we expect to see certain things in your assignment:

Use appropriate design patterns.

Proper logging across the API.

NICE-TO-HAVE:
Do you want to impress us? Show us what you got  . Here you have a list of topics to blow our minds:

TDD and/or BDD.

Proper use of Middlewares.

Generate the documentation of your API automatically from code.

Authentication.

Do you have experience in any cloud provider? If so, how would you deploy the API?