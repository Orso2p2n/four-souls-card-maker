"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.is64BitArch = void 0;
// List of Node.js-formatted 64 bit arches
const SIXTY_FOUR_BIT_ARCHES = ["arm64", "x64"];
/**
 * Determines whether the given architecture is a 64-bit arch.
 *
 * @param arch - a Node.js-style architecture name
 */
function is64BitArch(arch) {
    return SIXTY_FOUR_BIT_ARCHES.includes(arch);
}
exports.is64BitArch = is64BitArch;
//# sourceMappingURL=arch.js.map