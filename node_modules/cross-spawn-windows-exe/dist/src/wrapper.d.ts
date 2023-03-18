import { CrossSpawnArgs, CrossSpawnOptions } from "@malept/cross-spawn-promise";
export declare function canRunWindowsExeNatively(): boolean;
/**
 * The exception thrown when the wrapper command could not be found to execute.
 */
export declare class WrapperError extends Error {
    /**
     * @param wrapperCommand - The wrapper that tried to be executed
     * @param installInstructions - Instructions on how to install the wrapper
     */
    constructor(wrapperCommand: string, installInstructions?: string);
}
/**
 * A function which determines the wrapper path or binary to use in [[spawnWrapper]].
 *
 * @param customPath - The path specified by [[CrossSpawnExeOptions|CrossSpawnExeOptions.wrapperCommand]], usually
 * prioritized over other paths/binaries in the function.
 */
export declare type DetermineWrapperFunction = (customPath?: string) => string;
/**
 * An extension to `CrossSpawnOptions` to optionally specify a custom wrapper command and
 * instructions to install the wrapper.
 */
export declare type CrossSpawnExeOptions = CrossSpawnOptions & {
    /**
     * The path to a binary that wraps the called executable.
     *
     * Defaults to `wine64` or `wine`, depending on the host machine's architecture.
     */
    wrapperCommand?: string;
    /**
     * Instructions for installing the wrapper binary.
     */
    wrapperInstructions?: string;
};
/**
 * Determines if the specified command exists, either in the `PATH` environment variable or if the
 * absolute path exists.
 */
export declare function wrapperCommandExists(wrapperCommand: string): Promise<boolean>;
/**
 * A wrapper for `cross-spawn`'s `spawn` function that wraps the `cmd` with `wrapperCommand` if it
 * is specified.
 */
export declare function spawnWrapper(cmd: string, args?: CrossSpawnArgs, options?: CrossSpawnExeOptions): Promise<string>;
/**
 * A helper variant of [[spawnWrapper]] which uses a [[DetermineWrapperFunction]] to
 * determine `wrapperCommand`.
 */
export declare function spawnWrapperFromFunction(wrapperFunction: DetermineWrapperFunction, cmd: string, args?: CrossSpawnArgs, options?: CrossSpawnExeOptions): Promise<string>;
