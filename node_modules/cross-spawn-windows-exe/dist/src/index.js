"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.spawnExe = exports.exeDependencyInstallInstructions = exports.spawnDotNet = exports.dotNetDependencyInstallInstructions = exports.normalizePath = exports.is64BitArch = exports.WrapperError = exports.spawnWrapperFromFunction = exports.spawn = exports.canRunWindowsExeNatively = void 0;
var wrapper_1 = require("./wrapper");
Object.defineProperty(exports, "canRunWindowsExeNatively", { enumerable: true, get: function () { return wrapper_1.canRunWindowsExeNatively; } });
Object.defineProperty(exports, "spawn", { enumerable: true, get: function () { return wrapper_1.spawnWrapper; } });
Object.defineProperty(exports, "spawnWrapperFromFunction", { enumerable: true, get: function () { return wrapper_1.spawnWrapperFromFunction; } });
Object.defineProperty(exports, "WrapperError", { enumerable: true, get: function () { return wrapper_1.WrapperError; } });
var arch_1 = require("./arch");
Object.defineProperty(exports, "is64BitArch", { enumerable: true, get: function () { return arch_1.is64BitArch; } });
var normalize_path_1 = require("./normalize-path");
Object.defineProperty(exports, "normalizePath", { enumerable: true, get: function () { return normalize_path_1.normalizePath; } });
var dotnet_1 = require("./dotnet");
Object.defineProperty(exports, "dotNetDependencyInstallInstructions", { enumerable: true, get: function () { return dotnet_1.dotNetDependencyInstallInstructions; } });
Object.defineProperty(exports, "spawnDotNet", { enumerable: true, get: function () { return dotnet_1.spawnDotNet; } });
var exe_1 = require("./exe");
Object.defineProperty(exports, "exeDependencyInstallInstructions", { enumerable: true, get: function () { return exe_1.exeDependencyInstallInstructions; } });
Object.defineProperty(exports, "spawnExe", { enumerable: true, get: function () { return exe_1.spawnExe; } });
//# sourceMappingURL=index.js.map