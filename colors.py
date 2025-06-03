from itertools import product
import matplotlib.colors as mcolors


def luminance(r, g, b):
    a = [v / 255.0 for v in (r, g, b)]
    a = [v / 12.92 if v <= 0.03928 else ((v + 0.055) / 1.055) ** 2.4 for v in a]
    return 0.2126 * a[0] + 0.7152 * a[1] + 0.0722 * a[2]


def contrast_ratio(color1, color2):
    rgb1 = mcolors.to_rgb(color1)
    rgb2 = mcolors.to_rgb(color2)
    L1 = luminance(*(int(c * 255) for c in rgb1))
    L2 = luminance(*(int(c * 255) for c in rgb2))
    lighter = max(L1, L2)
    darker = min(L1, L2)
    return round((lighter + 0.05) / (darker + 0.05), 2)


colors = [
    "#0c0c0c",
    "#c50f1f",
    "#13a10e",
    "#c19c00",
    "#0037da",
    "#881798",
    "#3a96dd",
    "#cccccc",
    "#767676",
    "#e74856",
    "#16c60c",
    "#f9f1a5",
    "#3b78ff",
    "#b4009e",
    "#61d6d6",
    "#f2f2f2",
]

results = []
for fg, bg in product(colors, repeat=2):
    if fg != bg:
        ratio = contrast_ratio(fg, bg)
        results.append((ratio, fg, bg))

results.sort(reverse=True)

for ratio, fg, bg in results:
    print(f"Contrast {ratio:.2f}: FG {fg} on BG {bg}")
