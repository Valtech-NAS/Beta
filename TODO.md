# NAS Exemplar Tech Debt List #

Dev work that is not covered by backlog stories or TODO comments in the code. 

## Web layer ##

- refactor: controllers should use providers to avoid containing orchestration logic
- refactor: configuration service - use mongo collection with JSON fallback. This should periodically check for updates to allow in-situ config updates to running applications.
- validation messages should be parameterised where possible (e.g. max length). See AddressMessages.cs for an example.
- check all messages starting with "TODO" and 'TODO' (C# and JavaScript).
- usernamecheck should use remote validator
- review ApplyWebTrends attribute - use on controller or apply globally  
- increase the HSTS header to months or years in line with GDS recommendations when we're confident it works https://www.gov.uk/service-manual/operations/operating-servicegovuk-subdomains#transport-layer-security
- CDN changes:
    - fix links to CDN to use "assets" sub-domain in production
    - solve issue with custom domain name being used with an Azure CDN over HTTPS (may move CDN off Azure or use 3rd party CDN)
- refactor: _qualificationsJS.cshtml, _qualificationsNonJS.cshtml, _workExperiencesJS.cshtml, _workExperiencesNonJS.cshtml files are in ApprenticeshipApplication and TraineeshipApplication folders, but only differs in the model they receive. Tried to use the base class but HasQualification and HasWorkExperience property doesn't propagate correctly to the controller.
- Razor view unit tests (Create example use)
- Potentially remove IsWebsiteOffline/WebsiteOfflineMessage

## Service layer ##

- Lock down providers to "internal", web, application and infrastructure to enforce structural pattern, see [http://msdn.microsoft.com/en-us/library/system.runtime.compilerservices.internalsvisibletoattribute%28v=vs.110%29.aspx](http://msdn.microsoft.com/en-us/library/system.runtime.compilerservices.internalsvisibletoattribute%28v=vs.110%29.aspx) for application providers

## Infrastructure layer ##

- log request/response payloads for nas gateway calls
- change to entity repos (re. Mark). E.g. Consider renaming GenericMongoClient to MongoRepositoryBase; move MongoDB code out of into new MongoClient class; MongoRepositoryBase (and other future repos that may not be based on Domain EntityBase) would consume MongoClient (via IoC).
- wrap ElasticSearchClient -> search into a new class to be able to rethrow WebException swallowed by Nest
- agree on and implement location search behaviour fix for prefix sorting on different servers
- refactor: vacancy ETL process (simplify to process pages immediately? TBD. current approach facilitates multiple data sources)
- additional mongo indexes for app status and date updated (TBC: not sure how effective this would be - re. Alan)
- Multiple PreFetchCount values for each queue. Intetrnal processes should be able to have a much higher value than ones that talk to the gateway
- Enums should be serialized to strings into mongo (rather than numeric enum values)

## Cross cutting ##

- Error codes are defined inconsistently across the services. Codes should be considered part of a service's interface/contract so should be defined close to where the interface is defined
- Error logging is inconsistent between layers which leads to double logging/counting some errors/warnings. Need to decide on a consistent approach

## WebOps

- Configure remote powershell from Build Servers to Deployment Server to use Certificates over file system stored encrypted user details.   
- Merge build and management networks (See Simon)


----------

# Done #

- candidate registration should be queued (need to consider applying if not registered)
- replace AD with user auth repo
- some website URLs need to be reviewed to be more "friendly". e.g. vacancy detail should be /vacancy/12345 not /vacancysearch/details/446897
- controller actions should provide caching hints
- refactor azure message queue types
- integrate revised vacancy summary service
- integrate revised vacancy detail service
- integrate application update service
- integrate gateway certificates
- logging levels should be used in accordance with article on wiki
- logging should be called consistently across components (i.e. volume of log entries)
- logging should include an identifier which can be used to correlate a user's activity during a session (NLog MDC)
- remove legacy reference data service
- controller actions should use async where possible
- validation summary links need to be clicked twice
- ensure autosave interval and other settings are set to production values
- need to trim user input data, e.g. http://stackoverflow.com/questions/1718501/asp-net-mvc-best-way-to-trim-strings-after-data-entry-should-i-create-a-custo
- need to consider turning off integration tests against NAS Gateway services once we are hitting the live service
- solution should be executable when disconnected from platform (i.e. standalone)
- demo website should use separate configuration (e.g. databases, settings, etc.)

# Descoped #

- write bundle orderer for bundle.config
- replace address lookup with public service. No longer available - need to investigate other options with the agency
