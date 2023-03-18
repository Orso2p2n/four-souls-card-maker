import { CrossSpawnArgs } from "@malept/cross-spawn-promise";
import { CrossSpawnExeOptions } from "./wrapper";
/**
 * Installation instructions for dependencies related to running Windows executables on the
 * host platform (i.e., Wine on non-Windows platforms).
 */
export declare function exeDependencyInstallInstructions(): string;
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
export declare function determineWineWrapper(customWinePath?: string): string;
/**
 * Spawn a Windows executable. On non-Windows platforms, use [Wine](https://www.winehq.org/)
 * to run it.
 */
export declare function spawnExe(cmd: string, args?: CrossSpawnArgs, options?: CrossSpawnExeOptions): Promise<string>;
