import { useState, useEffect } from 'react';

export const LoadingWidget = () => {
  const [showText, setShowText] = useState(false);
  
  useEffect(() => {
    const timer = setTimeout(() => {
      setShowText(true);
    }, 500);
    
    return () => clearTimeout(timer);
  }, []);
  
  return (
    <div className="fixed inset-0 flex items-center justify-center bg-white/80 z-50">
      <div>{showText ? 'Loading' : ''}</div>
    </div>
  );
};