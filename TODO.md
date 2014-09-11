# NAS Exemplar Tech Debt List #

Dev work that is not covered by backlog stories or TODO comments in the code. 

## Web layer ##

- controllers should use providers to avoid containing orchestration logic
- controller actions should use async where possible
- controller actions should provide caching hints
- qualification types reference data should be read from configuration / provided by a configuration service
- some website URLs need to be reviewed to be more "friendly"
- validation summary links need to be clicked twice
- validation messages should be parameterised where possible (e.g. max length). See AddressMessages.cs for an example.
- check all messages starting with "TODO" and 'TODO' (C# and JavaScript).
- usernamecheck should use remote validator
- write bundle orderer for bundle.config
- need to trim user input data, e.g. http://stackoverflow.com/questions/1718501/asp-net-mvc-best-way-to-trim-strings-after-data-entry-should-i-create-a-custo
- need to consider turning off integration tests against NAS Gateway services once we are hitting the live service

## Service layer ##

- remove legacy reference data service
- candidate registration should be queued
- refactor azure message queue types

## Infrastructure layer ##

- change to entity repos (re. Mark)
- integrate revised vacancy summary service
- integrate revised vacancy detail service
- integrate application update service
- integrate gateway certificates
- wrap ElasticSearchClient -> search into a new class to be able to rethrow WebException swallowed by Nest.

## Cross cutting ##

- solution should be executable when disconnected from platform (i.e. standalone)
- logging levels should be used in accordance with article on wiki
- logging should be called consistently across components (i.e. volume of log entries)
- demo website should use separate configuration (e.g. databases)
- logging should include an identifier which can be used to correlate a user's activity during a session (Nlog MDC)

----------

# Done #

(add items here from above)
