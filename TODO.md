# NAS Exemplar Tech Debt List #

Dev work that is not covered by backlog stories or TODO comments in the code. 

## Web layer ##

- controllers should use providers to avoid containing orchestration logic
- qualification types reference data should be read from configuration / provided by a configuration service
- some website URLs need to be reviewed to be more "friendly"
- validation messages should be parameterised where possible (e.g. max length). See AddressMessages.cs for an example.
- check all messages starting with "TODO" and 'TODO' (C# and JavaScript).
- usernamecheck should use remote validator
- write bundle orderer for bundle.config

## Service layer ##

- candidate registration should be queued (need to consider applying if not registered)

## Infrastructure layer ##

- change to entity repos (re. Mark)
- wrap ElasticSearchClient -> search into a new class to be able to rethrow WebException swallowed by Nest

## Cross cutting ##

- solution should be executable when disconnected from platform (i.e. standalone)
- demo website should use separate configuration (e.g. databases, settings, etc.)

----------

# Done #

- controller actions should provide caching hints
- refactor azure message queue types
- integrate revised vacancy summary service
- integrate revised vacancy detail service
- integrate application update service
- integrate gateway certificates
- logging levels should be used in accordance with article on wiki
- logging should be called consistently across components (i.e. volume of log entries)
- logging should include an identifier which can be used to correlate a user's activity during a session (Nlog MDC)
- remove legacy reference data service
- controller actions should use async where possible
- validation summary links need to be clicked twice
- ensure autosave interval and other settings are set to production values
- need to trim user input data, e.g. http://stackoverflow.com/questions/1718501/asp-net-mvc-best-way-to-trim-strings-after-data-entry-should-i-create-a-custo
- need to consider turning off integration tests against NAS Gateway services once we are hitting the live service
