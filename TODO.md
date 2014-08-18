# NAS Exemplar Tech Debt List #

Dev work that is not covered by backlog stories or TODO comments in the code. 

## Web layer ##

- controllers should use providers and to avoid containing orchestration logic
- controller actions should use async where possible
- controller actions should provide caching hints
- qualification types reference data should be read from configuration / provided by a configuration service
- some website URLs need to be reviewed to be more "friendly"

## Service layer ##

- remove legacy reference data service
- candidate registration should be queued

## Infrastructure layer ##

- change to entity repos (re. Mark)

## Cross cutting ##

- solution should be executable when disconnected from platform (i.e. standalone)
- logging levels should be used in accordance with article on wiki
- logging should be called consistently across components (i.e. volume of log entries)
- demo website should use separate configuration (e.g. databases)

----------

# Done #

(add items here from above)
