## OpSec Features and Considerations

The agent does not have offensive elevation and persistence capabilities and is meant to be launched as part of a larger offensive effort whereby the stages and implants create ecosystem for it's invocation. In the future hot module loading may help with that but initial staging is not the primary feature of the RNS agent. Some opsec capabilities that fall into ransomware operation are outline below:

- Hide console
```
A: .\FileConnector.exe -d -l c -s
```
- Check-in interval modification
- Timeout on async requests
- Hot reload of comms
- TBD: fallback server discovery, currently static options.
- In memory logging and retrieval

  
