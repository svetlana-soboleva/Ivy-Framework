#!/usr/bin/env python3

import os
import re
import sys

def update_ingress_tags(file_path):
    """Update Ingress tags from old format to new format"""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Pattern to match <Ingress Text="..." />
        pattern = r'<Ingress Text="([^"]*)" />'
        
        def replacement(match):
            text = match.group(1)
            return f'<Ingress>\n{text}\n</Ingress>'
        
        updated_content = re.sub(pattern, replacement, content)
        
        # Only write if content changed
        if updated_content != content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(updated_content)
            print(f"Updated: {file_path}")
            return True
        else:
            print(f"No changes needed: {file_path}")
            return False
            
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return False

def main():
    # Get all markdown files that contain old Ingress format
    docs_dir = "Docs"
    updated_count = 0
    
    for root, dirs, files in os.walk(docs_dir):
        for file in files:
            if file.endswith('.md'):
                file_path = os.path.join(root, file)
                try:
                    with open(file_path, 'r', encoding='utf-8') as f:
                        content = f.read()
                    
                    if '<Ingress Text=' in content:
                        if update_ingress_tags(file_path):
                            updated_count += 1
                except Exception as e:
                    print(f"Error reading {file_path}: {e}")
    
    print(f"\nTotal files updated: {updated_count}")

if __name__ == "__main__":
    main()
