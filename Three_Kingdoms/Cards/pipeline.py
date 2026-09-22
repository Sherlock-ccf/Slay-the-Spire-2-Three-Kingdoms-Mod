import os
import glob


def process_cs_file(filepath):
    filename = os.path.splitext(os.path.basename(filepath))[0]

    with open(filepath, 'r', encoding='utf-8') as f:
        lines = f.readlines()

    modified = False

    # Step 1: add 'using Three_Kingdoms.Node;' before namespace if missing
    has_node_using = any('using Three_Kingdoms.Node;' in line for line in lines)
    if not has_node_using:
        for i, line in enumerate(lines):
            if line.strip().startswith('namespace '):
                lines.insert(i, 'using Three_Kingdoms.Node;\n')
                modified = True
                break

    # Step 2: add SfxPath after the first '{' if missing
    sfx_line = f'\tpublic string SfxPath => $"res://Three_Kingdoms/sfx/{{nameof({filename})}}.mp3";\n'
    has_sfx = any('SfxPath' in line for line in lines)
    if not has_sfx:
        for i, line in enumerate(lines):
            if line.strip() == '{':
                lines.insert(i + 1, sfx_line)
                modified = True
                break

    # Step 3: add CardPlayer.PlayCardSfx(SfxPath); after the first '{' following OnPlay if missing
    has_play_sfx = any('CardPlayer.PlayCardSfx(SfxPath);' in line for line in lines)
    if not has_play_sfx:
        onplay_idx = None
        for i, line in enumerate(lines):
            if 'OnPlay' in line:
                onplay_idx = i
                break

        if onplay_idx is not None:
            for i in range(onplay_idx, len(lines)):
                if lines[i].strip() == '{':
                    lines.insert(i + 1, '\t\tCardPlayer.PlayCardSfx(SfxPath);\n')
                    modified = True
                    break

    if modified:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.writelines(lines)
        print(f'[MODIFIED] {filename}.cs')
    else:
        print(f'[SKIPPED] {filename}.cs')

    return modified


def main():
    script_dir = os.path.dirname(os.path.abspath(__file__))
    cs_files = glob.glob(os.path.join(script_dir, '**', '*.cs'), recursive=True)

    if not cs_files:
        print('No .cs files found.')
        return

    modified_count = 0
    skip_count = 0

    for filepath in sorted(cs_files):
        if process_cs_file(filepath):
            modified_count += 1
        else:
            skip_count += 1

    print(f'\nDone. {modified_count} file(s) modified, {skip_count} file(s) skipped.')


if __name__ == '__main__':
    main()
