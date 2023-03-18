import { CrossSpawnArgs } from "@malept/cross-spawn-promise";
import { CrossSpawnExeOptions } from "./wrapper";
/**
 * Installation instructions for dependencies related to running .NET executables on the
 * host platform (i.e., Mono on non-Windows platforms).
 */
export declare function dotNetDependencyInstallInstructions(): string;
/**
 * Heuristically determine the path to `mono` to use.
 *
 * Method used to determine the path:
 *
 * 1. `customDotNetPath`, if provided to the function
 * 2. The `MONO_BINARY` environment variable, if set and non-empty
 * 3. `mono` found by searching the directories in the `PATH` environment variable
 */
export declare function determineDotNetWrapper(customDotNetPath?: string): string;
/**
 * Spawn a .NET executable. On non-Windows platforms, use [Nono](https://www.mono-project.com/)
 * to run it.
 */
export declare function spawnDotNet(cmd: string, args?: CrossSpawnArgs, options?: CrossSpawnExeOptions): Promise<string>;
