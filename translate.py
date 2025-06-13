import subprocess
import re
from pathlib import Path
from deep_translator import GoogleTranslator

russian = re.compile('[\u0400-\u04FF]')
xml_tag_re = re.compile(r'>([^<]*[\u0400-\u04FF][^<]*)<')
string_literal_re = re.compile(r'([$@]?")([^"]*[\u0400-\u04FF][^"]*)(")')

def translate_text(text: str) -> str:
    try:
        return GoogleTranslator(source='ru', target='en').translate(text)
    except Exception:
        return text

def try_read_file(path: Path, encodings=['utf-8-sig']) -> str:
    for enc in encodings:
        try:
            return path.read_text(encoding=enc)
        except UnicodeDecodeError:
            continue
    raise UnicodeDecodeError(f"Wrong encoding.")

def process_comment_line(line: str) -> str:
    comment_match = re.search(r'(\s*//+)(\s*)(.*)', line)
    if comment_match and russian.search(comment_match.group(3)):
        prefix, spacing, comment = comment_match.groups()
        translated = translate_text(comment.strip())
        return f'{prefix}{spacing}{translated}'
    
    hash_match = re.search(r'(\s*#)(\s*)(.*)', line)
    if hash_match and russian.search(hash_match.group(3)):
        prefix, spacing, comment = hash_match.groups()
        translated = translate_text(comment.strip())
        return f'{prefix}{spacing}{translated}'

    return line

def process_file(path: Path) -> bool:
    print(f'Translating {path}')
    lines = try_read_file(path).splitlines()
    changed = False
    new_lines = []
    for line in lines:
        newline = line
        if russian.search(line):
            newline = xml_tag_re.sub(lambda m: '>' + translate_text(m.group(1)) + '<', newline)
            
            newline = process_comment_line(newline)
            
            newline = string_literal_re.sub(lambda m: m.group(1) + translate_text(m.group(2)) + m.group(3), newline)
        if newline != line:
            changed = True
        new_lines.append(newline)
    if changed:
        path.write_text('\n'.join(new_lines) + '\n', encoding='utf-8')
    return changed


def main() -> None:
    result = subprocess.run(['git', 'ls-files'], capture_output=True, text=True, check=True)
    tracked_files = result.stdout.splitlines()
    for file_path in tracked_files:
        if file_path.endswith('.py') or file_path.endswith('.cs'):
            process_file(Path(file_path))


if __name__ == '__main__':
    main()