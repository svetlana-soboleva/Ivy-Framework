import { ComponentPropsWithoutRef } from 'react';

export default function IvyLogo({ className, ...props }: ComponentPropsWithoutRef<"svg">) {
    return (
        <svg 
            xmlns="http://www.w3.org/2000/svg" 
            version="1.1" 
            viewBox="0 0 251.4 200.01"
            className={className}
            {...props}
        >
            <path fill="currentColor" d="M50.28,0H0v50.28c27.77,0,50.28-22.51,50.28-50.28Z"/>
            <path fill="currentColor" d="M50.28,50.28H0v100.56h50.28v-50.28c0,27.77,22.51,50.28,50.28,50.28h0c27.77,0,50.28-22.51,50.28-50.28,0,25.23,18.59,46.12,42.81,49.72-24.23,3.61-42.81,24.5-42.81,49.73h50.28c27.4,0,49.68-21.92,50.27-49.17h.01v-50.28h0v-50.28c-27.77,0-50.28,22.51-50.28,50.28,0-27.77-22.51-50.28-50.28-50.28s-50.28,22.51-50.28,50.28c0-27.77-22.51-50.28-50.28-50.28h0Z"/>
        </svg>
    );
}
