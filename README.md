## Coding Challenge


### Links
* VSTS CI: https://tonyxia.visualstudio.com/public/_build?definitionId=3
* VSTS CD: https://tonyxia.visualstudio.com/public/_releases2?definitionId=1&view=mine&_a=releases
* Demo Env(Azure App Service): https://wcc-demo.azurewebsites.net/api/progress
* Swagger Url: https://wcc-demo.azurewebsites.net/swagger


### `Progress` Endpoint
* Method: `GET`
* Url: `api/progress`
* Response Payload
```
[
    {
        "floor": *floor number*,
        "statusPercentage": {
            "notStarted": *percentage of not started jobs*,
            "inProgress": *percentage of in progress jobs*,
            "delayed": *percentage of delayed jobs*,
            "complete": *percentage of complete jobs*
        }
    }
]
```

### Design Considerations
* dotnet core 2.1
* EF core. Use InMemory DB for testing
* Minimize the code change of the floor progress algorithm, if DB introduces new job status in future
* The percentage numbers are rounded to two decimal places
* The sum of the percentages of each floor should be 1 (100%)
