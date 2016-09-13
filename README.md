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

### - add env-name key value -p password -s salt
Adds the supplied key & value entry to given env-name securely. The value is encrypted using the provided password and salt.
To add default secure value that applies to all environments, use `add default <key> <value> -p password -s salt`.
Please use the same password and salt for all secure keys.

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

Optionally pass `-p password -s salt` if secure keys needs to decrypted while applying.

### - help
Shows the usage instructions for this tool.

## Features in the backlog
* Plugin support to define data storage mechanism.
* Help command.
* Tests.
* Platform specific Executables.
* Make flags and named parameters order independent. e.g `add -p password -s salt key value` Nice to Have!.

