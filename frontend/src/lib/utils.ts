import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function getAppId():string|null { 
  const urlParams = new URLSearchParams(window.location.search);
  return urlParams.get('appId');
}

export function getAppArgs():string|null { 
  const urlParams = new URLSearchParams(window.location.search);
  return urlParams.get('appArgs');
}

export function getParentId():string|null { 
  const urlParams = new URLSearchParams(window.location.search);
  return urlParams.get('parentId');
}

export function getMachineId() {
  let machineId = localStorage.getItem('machineId');
  if (!machineId) {
      machineId = crypto.randomUUID();
      localStorage.setItem('machineId', machineId);
  }
  return machineId;
}

export function getIvyHost():string {
  const urlParams = new URLSearchParams(window.location.search);
  const ivyHost = urlParams.get('ivyHost');
  if (ivyHost) return ivyHost;

  const metaHost = document.querySelector('meta[name="ivy-host"]')?.getAttribute('content');
  if (metaHost) return metaHost;
  
  return window.location.origin;
}

export function camelCase(titleCase: any):any {
  if (typeof titleCase !== 'string') {
    return titleCase;
  }
  return titleCase.charAt(0).toLowerCase() + titleCase.slice(1);
}


