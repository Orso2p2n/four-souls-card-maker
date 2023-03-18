"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.spawnDotNet = exports.determineDotNetWrapper = exports.dotNetDependencyInstallInstructions = void 0;
const wrapper_1 = require("./wrapper");
/**
 * Installation instructions for dependencies related to running .NET executables on the
 * host platform (i.e., Mono on non-Windows platforms).
 */
function dotNetDependencyInstallInstructions() {
    switch (process.platform) {
        /* istanbul ignore next */
        case "win32":
            return "No wrapper necessary";
        case "darwin":
            return "Run `brew install mono` to install Mono on macOS via Homebrew.";
        case "linux":
            return "Consult your Linux distribution's package manager to determine how to install Mono.";
        /* istanbul ignore next */
        default:
            return "Consult your operating system's package manager to determine how to install Mono.";
    }
}
exports.dotNetDependencyInstallInstructions = dotNetDependencyInstallInstructions;
/**
 * Heuristically determine the path to `mono` to use.
 *
 * Method used to determine the path:
 *
 * 1. `customDotNetPath`, if provided to the function
 * 2. The `MONO_BINARY` environment variable, if set and non-empty
 * 3. `mono` found by searching the directories in the `PATH` environment variable
 */
function determineDotNetWrapper(customDotNetPath) {
    if (customDotNetPath) {
        return customDotNetPath;
    }
    if (process.env.MONO_BINARY) {
        return process.env.MONO_BINARY;
    }
    return "mono";
}
exports.determineDotNetWrapper = determineDotNetWrapper;
/**
 * Spawn a .NET executable. On non-Windows platforms, use [Nono](https://www.mono-project.com/)
 * to run it.
 */
async function spawnDotNet(cmd, args, options) {
    var _a;
    options !== null && options !== void 0 ? options : (options = {});
    (_a = options.wrapperInstructions) !== null && _a !== void 0 ? _a : (options.wrapperInstructions = dotNetDependencyInstallInstructions());
    return wrapper_1.spawnWrapperFromFunction(determineDotNetWrapper, cmd, args, options);
}
exports.spawnDotNet = spawnDotNet;
//# sourceMappingURL=dotnet.js.map