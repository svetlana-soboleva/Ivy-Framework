const plugin = require('tailwindcss/plugin');

const colors = {
  //name: [color, foreground, light, dark]
  "black":["#000000","#FFFFFF","#B5B5B5","#212221"],
  "white":["#FFFFFF","#000000","#FFFFFF","#6B6C6B"],
  "slate":["#6A7489","#000000","#CED1D7","#393D43"],
  "gray":["#6E727F","#000000","#CFD0D4","#3A3D40"],
  "zinc":["#717179","#FFFFFF","#D0D0D3","#3B3C3F"],
  "neutral":["#737373","#FFFFFF","#D0D0D0","#3C3D3C"],
  "stone":["#76716D","#FFFFFF","#D1D0CE","#3D3C3A"],
  "red":["#DD5860","#000000","#EFC5C9","#5D3235"],
  "orange":["#DC824D","#000000","#F1D4C1","#5E412E"],
  "amber":["#DEB145","#000000","#F3E4BA","#605129"],
  "yellow":["#E5E04C","#000000","#F5F4B3","#626125"],
  "lime":["#AFD953","#000000","#E3F2C0","#4F5F2E"],
  "green":["#86D26F","#000000","#D2EFCD","#405D3A"],
  "emerald":["#76CD94","#000000","#C2EEDA","#355A47"],
  "teal":["#5B9BA8","#000000","#BEDDE2","#2E4A4E"],
  "cyan":["#4469C0","#FFFFFF","#BACDE9","#293A55"],
  "sky":["#373BDA","#FFFFFF","#B8BCF1","#25295E"],
  "blue":["#381FF4","#FFFFFF","#B5ACF9","#231A66"],
  "indigo":["#4B28E2","#FFFFFF","#C0B4F4","#2C2161"],
  "violet":["#6637D1","#FFFFFF","#CABBEF","#36285B"],
  "purple":["#844CC0","#FFFFFF","#D4C2E9","#412F55"],
  "fuchsia":["#A361AF","#000000","#DFC9E4","#4B3750"],
  "pink":["#C377A0","#000000","#EAD1DF","#563E4B"],
  "rose":["#E48E91","#000000","#F4D8DA","#614647"],
  "primary":["#74C997","#000000","#C2EEDA","#355A47"],
  "secondary":["#C2CBC7","#000000","#EAEEEC","#565B58"],
  "destructive":["#DD5860","#000000","#EFC5C9","#5D3235"]
};


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
    plugin(function({ addBase }) {
      const cssVars = {};
      
      Object.entries(colors).forEach(([name, [color, foreground, light, dark]]) => {
        cssVars[`--${name}`] = color;
        cssVars[`--${name}-foreground`] = foreground;
        cssVars[`--${name}-light`] = light;
        cssVars[`--${name}-dark`] = dark;
      });
      
      addBase({
        ':root': cssVars
      });
    })
],
}

