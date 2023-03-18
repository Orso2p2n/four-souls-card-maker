"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.normalizePath = exports.convertUNIXPathToWindows = void 0;
const is_wsl_1 = __importDefault(require("is-wsl"));
const cross_spawn_promise_1 = require("@malept/cross-spawn-promise");
/**
 * Provides a human-friendly error message when `@malept/cross-spawn-promise` can't find `wslpath`.
 */
function updateWSLPathError(error) {
    if (error.code === "ENOENT" && error.syscall === "spawn wslpath") {
        error.message = `Could not find 'wslpath' in any of the directories listed in the PATH environment variable, which is needed to convert WSL paths to Windows-style paths.`;
    }
}
/**
 * Converts a UNIX-style path to a Windows-style path via `wslpath`, which should come with any
 * WSL distribution.
 */
async function convertUNIXPathToWindows(wslPath) {
    const output = await cross_spawn_promise_1.spawn("wslpath", ["-w", wslPath], {
        updateErrorCallback: updateWSLPathError,
    });
    return output.trim();
}
exports.convertUNIXPathToWindows = convertUNIXPathToWindows;
/**
 * Converts a UNIX-style path to a Windows-style path if run in a WSL environment.
 */
async function normalizePath(pathToNormalize) {
    if (is_wsl_1.default) {
        return convertUNIXPathToWindows(pathToNormalize);
    }
    return pathToNormalize;
}
exports.normalizePath = normalizePath;
//# sourceMappingURL=normalize-path.js.map