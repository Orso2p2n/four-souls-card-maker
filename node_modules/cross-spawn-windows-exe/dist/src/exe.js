"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.spawnExe = exports.determineWineWrapper = exports.exeDependencyInstallInstructions = void 0;
const wrapper_1 = require("./wrapper");
const arch_1 = require("./arch");
/**
 * Installation instructions for dependencies related to running Windows executables on the
 * host platform (i.e., Wine on non-Windows platforms).
 */
function exeDependencyInstallInstructions() {
    switch (process.platform) {
        /* istanbul ignore next */
        case "win32":
            return "No wrapper necessary";
        case "darwin":
            return "Run `brew install --cask wine-stable` to install 64-bit wine on macOS via Homebrew.";
        case "linux":
            return "Consult your Linux distribution's package manager to determine how to install Wine.";
        /* istanbul ignore next */
        default:
            return "Consult your operating system's package manager to determine how to install Wine.";
    }
}
exports.exeDependencyInstallInstructions = exeDependencyInstallInstructions;
/**
 * Heuristically determine the path to `wine` to use.
 *
 * Method used to determine the path:
 *
 * 1. `customWinePath`, if provided to the function
 * 2. The `WINE_BINARY` environment variable, if set and non-empty
 * 3. If the host architecture is x86-64, `wine64` found by searching the directories in the `PATH`
 *    environment variable
 * 4. `wine` found by searching the directories in the `PATH` environment variable
 */
function determineWineWrapper(customWinePath) {
    if (customWinePath) {
        return customWinePath;
    }
    if (process.env.WINE_BINARY) {
        return process.env.WINE_BINARY;
    }
    if (arch_1.is64BitArch(process.arch)) {
        return "wine64";
    }
    return "wine";
}
exports.determineWineWrapper = determineWineWrapper;
/**
 * Spawn a Windows executable. On non-Windows platforms, use [Wine](https://www.winehq.org/)
 * to run it.
 */
async function spawnExe(cmd, args, options) {
    var _a;
    options !== null && options !== void 0 ? options : (options = {});
    (_a = options.wrapperInstructions) !== null && _a !== void 0 ? _a : (options.wrapperInstructions = exeDependencyInstallInstructions());
    return wrapper_1.spawnWrapperFromFunction(determineWineWrapper, cmd, args, options);
}
exports.spawnExe = spawnExe;
//# sourceMappingURL=exe.js.map