const plugin = require('tailwindcss/plugin');

// import { timingSafeEqual } from 'crypto';

// const colors = require('tailwindcss/colors')

// const toRgba = (hexCode, opacity = 50) => {
//     let hex = hexCode.replace('#', '');
    
//     if (hex.length === 3) {
//         hex = `${hex[0]}${hex[0]}${hex[1]}${hex[1]}${hex[2]}${hex[2]}`;
//     }    
    
//     const r = parseInt(hex.substring(0, 2), 16);
//     const g = parseInt(hex.substring(2, 4), 16);
//     const b = parseInt(hex.substring(4, 6), 16);

//     return `rgba(${r},${g},${b},${opacity / 100})`;
// };

// const flattenColorPalette = (obj, sep='-') => Object.assign(
//     {}, 
//     ...function _flatten(o, p='') { 
//       return [].concat(...Object.keys(o)
//         .map(k => 
//           typeof o[k] === 'object' ?
//             _flatten(o[k],k+sep) : 
//             ({[p+k]: o[k]})
//         )
//       );
//     }(obj)
//   );

function flattenColors(obj, prefix = '') {
    let result = {};
    for (const [key, value] of Object.entries(obj)) {
      const newKey = prefix ? `${prefix}-${key}` : key;
      if (value && typeof value === 'object') {
        Object.assign(result, flattenColors(value, newKey));
      } else {
        result[newKey] = value;
      }
    }
    return result;
}

function toRgba(color) {
  if (color.startsWith('rgba')) {
    return color
      .replace('rgba(', '')
      .replace(')', '')
      .split(',')
      .map((value) => parseFloat(value.trim()));
  }
  if (color.startsWith('rgb')) {
    return color
      .replace('rgb(', '')
      .replace(')', '')
      .split(',')
      .map((value) => parseFloat(value.trim()))
      .concat([1]);
  }
  if (color.startsWith('#')) {
    const hex = color.replace('#', '');
    const bigint = parseInt(hex, 16);
    const r = (bigint >> 16) & 255;
    const g = (bigint >> 8) & 255;
    const b = bigint & 255;
    return [r, g, b, 1];
  }
  return null;
}

function luminance(color) {
  const [r, g, b, a] = toRgba(color);
  if (a < 1) return 0;
  const rsrgb = r / 255;
  const gsrgb = g / 255;
  const bsrgb = b / 255;
  const rLinear = rsrgb <= 0.03928 ? rsrgb / 12.92 : ((rsrgb + 0.055) / 1.055) ** 2.4;
  const gLinear = gsrgb <= 0.03928 ? gsrgb / 12.92 : ((gsrgb + 0.055) / 1.055) ** 2.4;
  const bLinear = bsrgb <= 0.03928 ? bsrgb / 12.92 : ((bsrgb + 0.055) / 1.055) ** 2.4;
  return 0.2126 * rLinear + 0.7152 * gLinear + 0.0722 * bLinear;
}

const colorVars = plugin(function ({ addBase, theme }) {
  const colors = theme('colors');
  const flattenedColors = flattenColors(colors);
  const cssVariables = Object.entries(flattenedColors)
    .filter(([key]) => key.endsWith('500'))
    .reduce((acc, [key, value]) => {
      const varName = key.replace(/-500$/, '');
      acc[`--${varName}`] = value;
      var luminanceValue = luminance(value);
      var twShade = luminanceValue > 0.5 ? '200' : '900';
      var contrastColor = colors[varName][twShade];
      acc[`--${varName}-foreground`] = contrastColor;
      return acc;
    }, {});

  addBase({
  ':root': cssVariables,
  });
});


/** @type {import('tailwindcss').Config} */
export default {
  darkMode: ["class"],
  content: [
    "./app.html",
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
  	extend: {
  		borderRadius: {
  			lg: 'var(--radius)',
  			md: 'calc(var(--radius) - 2px)',
  			sm: 'calc(var(--radius) - 4px)'
  		},
  		fontFamily: {
        mono: [
          'IBM Plex Mono',
          'monospace'
        ],
  			sans: [
  				'Inter',
  				'sans-serif'
  			]
  		},
  		colors: {
        background: 'var(--background)',
        foreground: 'var(--foreground)',
        card: {
          DEFAULT: 'var(--card)',
          foreground: 'var(--card-foreground)',
        },
        popover: {
          DEFAULT: 'var(--popover)',
          foreground: 'var(--popover-foreground)',
        },
        primary: {
          DEFAULT: 'var(--primary)',
          foreground: 'var(--primary-foreground)',
        },
        secondary: {
          DEFAULT: 'var(--secondary)',
          foreground: 'var(--secondary-foreground)',
        },
        muted: {
          DEFAULT: 'var(--muted)',
          foreground: 'var(--muted-foreground)',
        },
        accent: {
          DEFAULT: 'var(--accent)',
          foreground: 'var(--accent-foreground)',
        },
        destructive: {
          DEFAULT: 'var(--destructive)',
          foreground: 'var(--destructive-foreground)',
        },
        border: 'var(--border)',
        input: 'var(--input)',
        ring: 'var(--ring)',
        chart: {
          '1': 'var(--chart-1)',
          '2': 'var(--chart-2)',
          '3': 'var(--chart-3)',
          '4': 'var(--chart-4)',
          '5': 'var(--chart-5)',
        },
        sidebar: {
          DEFAULT: 'var(--sidebar-background)',
          foreground: 'var(--sidebar-foreground)',
          primary: 'var(--sidebar-primary)',
          'primary-foreground': 'var(--sidebar-primary-foreground)',
          accent: 'var(--sidebar-accent)',
          'accent-foreground': 'var(--sidebar-accent-foreground)',
          border: 'var(--sidebar-border)',
          ring: 'var(--sidebar-ring)',
        },
      },
          
  		keyframes: {
  			'accordion-down': {
  				from: {
  					height: '0'
  				},
  				to: {
  					height: 'var(--radix-accordion-content-height)'
  				}
  			},
  			'accordion-up': {
  				from: {
  					height: 'var(--radix-accordion-content-height)'
  				},
  				to: {
  					height: '0'
  				}
  			}
  		},
  		animation: {
  			'accordion-down': 'accordion-down 0.2s ease-out',
  			'accordion-up': 'accordion-up 0.2s ease-out'
  		}
  	}
  },
  plugins: [
    require("tailwindcss-animate"),
    require('@tailwindcss/typography'),
    colorVars

    // function ({ addUtilities, theme }) {
    //     const utilities = {
    //       '.bg-stripes': {
    //         backgroundImage:
    //           'linear-gradient(45deg, var(--stripes-color) 12.50%, transparent 12.50%, transparent 50%, var(--stripes-color) 50%, var(--stripes-color) 62.50%, transparent 62.50%, transparent 100%)',
    //         backgroundSize: '5.66px 5.66px',
    //       },
    //     }
  
    //     const addColor = (name, color) =>
    //       (utilities[`.bg-stripes-${name}`] = { '--stripes-color': color })
  
    //     const colors = flattenColorPalette(theme('backgroundColor'))
    //     for (let name in colors) {
    //       try {
    //         const [r, g, b, a] = toRgba(colors[name])
    //         if (a !== undefined) {
    //           addColor(name, colors[name])
    //         } else {
    //           addColor(name, `rgba(${r}, ${g}, ${b}, 0.4)`)
    //         }
    //       } catch (_) {
    //         addColor(name, colors[name])
    //       }
    //     }
  
    //     addUtilities(utilities)
    //   }
],
}

