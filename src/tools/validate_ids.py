#!/usr/bin/env python3
import json
import os
from pathlib import Path
from typing import Dict, List, Set
import glob

class IDValidator:
    def __init__(self, content_dir: str):
        self.content_dir = Path(content_dir)
        self.valid_ids: Dict[str, Set[str]] = {
            'skills': set(),
            'items': set(),
            'mobs': set(),
            'loot_tables': set(),
            'stat_templates': set(),
            'effects': set(),
            'talents': set(),
            'classes': set()
        }
        self.errors: List[str] = []

    def collect_ids(self):
        """Collect all valid IDs from JSON files."""
        # Collect skill IDs
        for skill_file in self.content_dir.glob('Skills/**/*.skill.json'):
            with open(skill_file) as f:
                data = json.load(f)
                if 'id' in data:
                    self.valid_ids['skills'].add(data['id'])

        # Collect item IDs
        for item_file in self.content_dir.glob('Item/**/*.item.json'):
            with open(item_file) as f:
                data = json.load(f)
                if 'id' in data:
                    self.valid_ids['items'].add(data['id'])

        # Collect Effect IDs
        for item_file in self.content_dir.glob('Effects/**/*.effect.json'):
            with open(item_file) as f:
                data = json.load(f)
                if 'id' in data:
                    self.valid_ids['effects'].add(data['id'])

        # Collect mob IDs
        for mob_file in self.content_dir.glob('Mob/**/*.mob.json'):
            with open(mob_file) as f:
                data = json.load(f)
                if 'id' in data:
                    self.valid_ids['mobs'].add(data['id'])

        # Collect loot table IDs
        for loot_file in self.content_dir.glob('LootTable/**/*.loot_table.json'):
            with open(loot_file) as f:
                data = json.load(f)
                if 'id' in data:
                    self.valid_ids['loot_tables'].add(data['id'])

        # Collect stat template IDs
        for stat_file in self.content_dir.glob('StatTemplate/**/*.stat_template.json'):
            with open(stat_file) as f:
                data = json.load(f)
                if 'id' in data:
                    self.valid_ids['stat_templates'].add(data['id'])

        # Collect talents IDs
        for stat_file in self.content_dir.glob('Talents/**/*.talent.json'):
            with open(stat_file) as f:
                data = json.load(f)
                if 'id' in data:
                    self.valid_ids['talents'].add(data['id'])
        
        # Collect classes IDs
        for stat_file in self.content_dir.glob('Classes/**/*.class.json'):
            with open(stat_file) as f:
                data = json.load(f)
                if 'id' in data:
                    self.valid_ids['classes'].add(data['id'])

    def validate_references(self):
        """Validate all references in JSON files."""
        # Validate mob files
        for mob_file in self.content_dir.glob('Mob/**/*.mob.json'):
            with open(mob_file) as f:
                data = json.load(f)
                
                # Validate skills
                if 'skills' in data:
                    skills = data['skills']
                    if isinstance(skills, list):
                        for skill in skills:
                            if skill not in self.valid_ids['skills']:
                                self.errors.append(f"Invalid skill ID '{skill}' referenced in {mob_file}")
                    elif isinstance(skills, str) and skills not in self.valid_ids['skills']:
                        self.errors.append(f"Invalid skill ID '{skills}' referenced in {mob_file}")

                # Validate loot table
                if 'loot_table' in data:
                    loot_table = data['loot_table']
                    if isinstance(loot_table, str) and loot_table not in self.valid_ids['loot_tables']:
                        self.errors.append(f"Invalid loot table ID '{loot_table}' referenced in {mob_file}")

                # Validate stats
                if 'stats' in data:
                    stats = data['stats']
                    if isinstance(stats, str) and stats not in self.valid_ids['stat_templates']:
                        self.errors.append(f"Invalid stat template ID '{stats}' referenced in {mob_file}")

        # Validate loot table files
        for loot_file in self.content_dir.glob('LootTable/**/*.loot.json'):
            with open(loot_file) as f:
                data = json.load(f)
                if 'entries' in data:
                    for entry in data['entries']:
                        if 'item_id' in entry and entry['item_id'] not in self.valid_ids['items']:
                            self.errors.append(f"Invalid item ID '{entry['item_id']}' referenced in {loot_file}")

        # Validate skill files
        for skill_file in self.content_dir.glob('Skills/**/*.skill.json'):
            with open(skill_file) as f:
                data = json.load(f)
                if 'effects' in data:
                    for effect in data['effects']:
                        if effect.get('type') == 'apply_effect' and 'effect_id' in effect:
                            if effect['effect_id'] not in self.valid_ids['effects']:
                                self.errors.append(f"Invalid effect ID '{effect['effect_id']}' referenced in {skill_file}")

    def run(self) -> bool:
        """Run the validation and return True if no errors were found."""
        print("Collecting valid IDs...")
        self.collect_ids()
        
        print("\nFound valid IDs:")
        for id_type, ids in self.valid_ids.items():
            print(f"{id_type}: {len(ids)} IDs")
            if ids:
                print(f"  Examples: {', '.join(list(ids)[:3])}")
        
        print("\nValidating references...")
        self.validate_references()
        
        if self.errors:
            print("\nValidation errors found:")
            for error in self.errors:
                print(f"- {error}")
            return False
        
        print("\nNo validation errors found!")
        return True

def main():
    content_dir = Path(__file__).parent.parent / 'Content'
    validator = IDValidator(content_dir)
    success = validator.run()
    exit(0 if success else 1)

if __name__ == '__main__':
    main() 