# cit
Tool to manage environment based configurations.

# Setup and Build
Install dotnet core, run `dotnet restore && dotnet build`.

# Usage
## Commands available
### - init
Initiates cit default environment and metadata.

### - init env-name
Initiates env-name. Initiates cit default environment, if not found.

### - add env-name key value
Adds the supplied key & value entry to given env-name.
To add default value that applies to all environments, use `add default <key> <value>`.

### - remove env-name key
Removes the key entry from the given env-name.

### - clean env-name
Removes the supplied env-name and its metadata.

### - copy existing-env-name new-env-name
Creates new-env-name environment with the same key & value entries as of existing-env-name.

### - apply env-name-1 env-name-2 ... env-name-n -f file-name-1 file-name-2 ... file-name-3
Apply does the following,
  * Merges the key-values from the supplied list of environments. Value available in last specified environment takes precedence.
  * Replaces the occurences of **#key** with its corresponding value for the set of files specified.

### - help
Shows the usage instructions for this tool.

## Features in the backlog
* Enhance existing functionality and fix gaps.
* Ability to have secure values.
* Plugin support to define data storage mechanism.

