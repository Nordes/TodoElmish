# Todo Elmish

In summary, it creates the common TODO example using the SAFE Stack in a fulma way. You can add, complete, delete or put back the in progress to pending state.

## Install pre-requisites

You'll need to install the following pre-requisites in order to build SAFE applications

* The [.NET Core SDK](https://www.microsoft.com/net/download)
* The [Yarn](https://yarnpkg.com/lang/en/docs/install/) package manager (you can also use `npm` but the usage of `yarn` is encouraged).
* [Node LTS](https://nodejs.org/en/download/) installed for the front end components.
* If you're running on OSX or Linux, you'll also need to install [Mono](https://www.mono-project.com/docs/getting-started/install/).

## Work with the application

```bash
dotnet tool restore
# Or use simply VS Code and start.
dotnet fake build -t run
```

# License
None... because it's only 300 lines... come on :).