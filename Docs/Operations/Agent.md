## Agent features and operation

RNS agent is an executable which is deployed on an asset for the purpose of carrying out tasks sent to it from the base (C2). 

The primary job of the RNS agent is to perform:
- Local or networked encryption and decryption of files within an environment.
- Managing one or several network account credentials.
- Managing connectivity to the base.

Secondary tasks include:
- Revealing agent presence by signaling a console or launching a payment collection or notification application.
- Managing run logs
- Self-termination and cleanup
  
Currently encryption and decryption is performed with a set of pre-shared keys, loaded as part of the policy into the agent.

The encryption keys can be specified per host, or per file, which allows for a more granular approach for the offensive team to manage decryption in the field after a simulated attack. 

The run of the policy is triggered by providing a correct master key which ensures proper authentication of the intent and serving as key for managing shielded account credentials to avoid leaks. 

RNS Agent is governed by a policy which is comprised of a set of configuration directives used for both the operation of the agent and it's interaction with the assets it is meant to encrypt. The policy can be dynamically pushed to the agent by the base, or it can be embedded into the agent for certain lights out scenarios.

More on [Policy](Pollcy.md)

The agent is assigned a site id which is used to identify to the base and serves as part of the key structure for encryption operations.

The RNS agent has a set of operational security features that may help with a tighter alignment with emulated ransomware.

More on [OpSec](OpSec.md)

The agent has a secondary (add-on) feature to accept a limited set of commands from a local IPc client in addition or as an alternative to reaching out to base.

More on [IPC client](IPC.md)

## Invocation

Method 1: Lights out background operation
 - Launch as a regular application by double clicking the exe
 This is useful for most cases.

Method 2: Console logs and options.
 This is useful for local in-field debugging, or for SOC.
  > Note: verbosity of logs can be set remotely by the base as well. Additionally, logs can be collected in memory and sent to the base.
  More on that in [Base](Base.md).

Usage:
```sh

  -c, --cfg        Config file
                   Example: -c config_file

  -p, --pipe       IPC Pipe name.

  -d, --debug      Debug output.

  -l, --log        (Default: m) Log to source: (m)emory, (c)onsole
                   Example: -l m

  -s, --showcon    (Default: false) Show hidden console
```

Example:
```sh
.\FileConnector.exe -d -l c -s

2021.02.21 11:25:33.695 Debug   opts.cmdIPCPipe is not set, defaulting to rns
2021.02.21 11:25:33.698 Debug   Log path is NOT specified. Embedded. ......
2021.02.21 11:25:33.704 Debug   Hiding Console: (0)
2021.02.21 11:25:33.704 Debug   Initializing ConfigLoader
2021.02.21 11:25:33.705 Debug   Loading config from embedded resources
2021.02.21 11:25:33.817 Debug   Deserializing config
2021.02.21 11:25:33.946 Debug   Config tree populated
2021.02.21 11:25:33.947 Debug   Starting in OpMode: Distributed
2021.02.21 11:25:33.948 Debug   Initializing HTTP
2021.02.21 11:25:33.949 Debug   Using Ident: Base1 for comms
2021.02.21 11:25:33.949 Debug   localhost:3001 (TSL: True) comms
```
