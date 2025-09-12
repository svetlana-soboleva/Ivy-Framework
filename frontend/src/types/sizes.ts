/**
 * Predefined size variants for consistent widget sizing across the framework.
 */
export enum Sizes {
  /**
   * Standard medium size (default).
   */
  Medium = 'Medium',

  /**
   * Compact small size for dense layouts.
   */
  Small = 'Small',

  /**
   * Prominent large size for emphasis.
   */
  Large = 'Large',
}

// Map Sizes enum to UI component size strings
export const mapSizeToUISize = (size: Sizes): 'Default' | 'Small' | 'Large' => {
  switch (size) {
    case Sizes.Small:
      return 'Small';
    case Sizes.Large:
      return 'Large';
    case Sizes.Medium:
    default:
      return 'Default';
  }
};
