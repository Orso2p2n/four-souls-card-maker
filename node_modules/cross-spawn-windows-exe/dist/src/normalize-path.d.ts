/**
 * Converts a UNIX-style path to a Windows-style path via `wslpath`, which should come with any
 * WSL distribution.
 */
export declare function convertUNIXPathToWindows(wslPath: string): Promise<string>;
/**
 * Converts a UNIX-style path to a Windows-style path if run in a WSL environment.
 */
export declare function normalizePath(pathToNormalize: string): Promise<string>;
