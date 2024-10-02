This file explains how Visual Studio created the project.

The following tools were used to generate this project:
- create-vite

The following steps were used to generate this project:
- Create react project with create-vite: `npm init --yes vite@latest my-portfolio-manager -- --template=react-ts`.
- Create project file (`my-portfolio-manager.esproj`).
- Create `launch.json` to enable debugging.
- Create `nuget.config` to specify location of the JavaScript Project System SDK (which is used in the first line in `my-portfolio-manager.esproj`).
- Add project to solution.
- Write this file.
