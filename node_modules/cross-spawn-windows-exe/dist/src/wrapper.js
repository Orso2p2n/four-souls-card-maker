"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.spawnWrapperFromFunction = exports.spawnWrapper = exports.wrapperCommandExists = exports.WrapperError = exports.canRunWindowsExeNatively = void 0;
const cross_spawn_promise_1 = require("@malept/cross-spawn-promise");
const fs = __importStar(require("fs"));
const is_wsl_1 = __importDefault(require("is-wsl"));
const path = __importStar(require("path"));
const which_1 = __importDefault(require("which"));
function canRunWindowsExeNatively() {
    return process.platform === "win32" || is_wsl_1.default;
}
exports.canRunWindowsExeNatively = canRunWindowsExeNatively;
/**
 * The exception thrown when the wrapper command could not be found to execute.
 */
class WrapperError extends Error {
    /**
     * @param wrapperCommand - The wrapper that tried to be executed
     * @param installInstructions - Instructions on how to install the wrapper
     */
    constructor(wrapperCommand, installInstructions) {
        const message = `Wrapper command '${wrapperCommand}' not found on the system.${installInstructions ? " " + installInstructions : ""}`;
        super(message);
    }
}
exports.WrapperError = WrapperError;
/**
 * Determines if the specified command exists, either in the `PATH` environment variable or if the
 * absolute path exists.
 */
async function wrapperCommandExists(wrapperCommand) {
    if (path.isAbsolute(wrapperCommand)) {
        return fs.existsSync(wrapperCommand);
    }
    else {
        try {
            await which_1.default(wrapperCommand);
            return true;
        }
        catch (_a) {
            return false;
        }
    }
}
exports.wrapperCommandExists = wrapperCommandExists;
/**
 * A wrapper for `cross-spawn`'s `spawn` function that wraps the `cmd` with `wrapperCommand` if it
 * is specified.
 */
async function spawnWrapper(cmd, args, options) {
    options !== null && options !== void 0 ? options : (options = {});
    const { wrapperCommand, wrapperInstructions, ...crossSpawnOptions } = options;
    if (wrapperCommand) {
        if (!(await wrapperCommandExists(wrapperCommand))) {
            throw new WrapperError(wrapperCommand, wrapperInstructions);
        }
        const augmentedArgs = args ? [cmd, ...args] : [cmd];
        return cross_spawn_promise_1.spawn(wrapperCommand, augmentedArgs, crossSpawnOptions);
    }
    return cross_spawn_promise_1.spawn(cmd, args, crossSpawnOptions);
}
exports.spawnWrapper = spawnWrapper;
/**
 * A helper variant of [[spawnWrapper]] which uses a [[DetermineWrapperFunction]] to
 * determine `wrapperCommand`.
 */
async function spawnWrapperFromFunction(wrapperFunction, cmd, args, options) {
    let exeOptions = options;
    if (!canRunWindowsExeNatively()) {
        const wrapperCommand = wrapperFunction(options === null || options === void 0 ? void 0 : options.wrapperCommand);
        exeOptions = options ? { ...options, wrapperCommand } : { wrapperCommand };
    }
    return spawnWrapper(cmd, args, exeOptions);
}
exports.spawnWrapperFromFunction = spawnWrapperFromFunction;
//# sourceMappingURL=wrapper.js.map