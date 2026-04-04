export namespace ColorUtils {
	// Types
	export type RGB = { r: number; g: number; b: number };
	export type HSL = { h: number; s: number; l: number };

	// Math
	export function clamp(v: number, min = 0, max = 100): number {
		return Math.min(max, Math.max(min, v));
	}

	// Conversions
	export function hexToRgb(hex: string): RGB | null {
		const m = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex.trim());
		if (!m) return null;
		return { r: parseInt(m[1], 16), g: parseInt(m[2], 16), b: parseInt(m[3], 16) };
	}

	export function rgbToHex(r: number, g: number, b: number): string {
		return (
			"#" +
			[r, g, b]
				.map((x) => {
					const h = x.toString(16);
					return h.length === 1 ? "0" + h : h;
				})
				.join("")
		);
	}

	export function rgbToHsl(r: number, g: number, b: number): HSL {
		r /= 255; g /= 255; b /= 255;
		const max = Math.max(r, g, b), min = Math.min(r, g, b);
		let h = 0, s = 0;
		const l = (max + min) / 2;

		if (max !== min) {
			const d = max - min;
			s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
			switch (max) {
				case r: h = (g - b) / d + (g < b ? 6 : 0); break;
				case g: h = (b - r) / d + 2; break;
				default: h = (r - g) / d + 4; break;
			}
			h /= 6;
		}
		return { h: h * 360, s: s * 100, l: l * 100 };
	}

	export function hslToRgb(h: number, s: number, l: number): RGB {
		h /= 360; s /= 100; l /= 100;
		if (s === 0) {
			const v = Math.round(l * 255);
			return { r: v, g: v, b: v };
		}
		const hue2rgb = (p: number, q: number, t: number) => {
			if (t < 0) t += 1;
			if (t > 1) t -= 1;
			if (t < 1 / 6) return p + (q - p) * 6 * t;
			if (t < 1 / 2) return q;
			if (t < 2 / 3) return p + (q - p) * (2 / 3 - t) * 6;
			return p;
		};
		const q = l < 0.5 ? l * (1 + s) : l + s - l * s;
		const p = 2 * l - q;
		const r = hue2rgb(p, q, h + 1 / 3);
		const g = hue2rgb(p, q, h);
		const b = hue2rgb(p, q, h - 1 / 3);
		return { r: Math.round(r * 255), g: Math.round(g * 255), b: Math.round(b * 255) };
	}


	// Validation & normalization
	export function normalizeHex6(hex: string): string | null {
		let t = hex.trim().replace(/^#/, '');
		if (/^[0-9a-f]{3}$/i.test(t)) {
			t = t.split('').map((c) => c + c).join('');
		}
		if (!/^[0-9a-f]{6}$/i.test(t)) return null;
		return ('#' + t).toUpperCase();
	}

	export function isHex6(t: string): boolean {
		return /^#([a-fA-F0-9]{6})$/.test(t);
	}

	// Ops

	export function contrastTextColor(bgHex: string, dark = "#171717", light = "#FFFFFF"): string {
		const rgb = hexToRgb(bgHex);
		if (!rgb) return dark;
		const l = 0.2126 * (rgb.r / 255) + 0.7152 * (rgb.g / 255) + 0.0722 * (rgb.b / 255);
		return l > 0.6 ? dark : light;
	}
	export function adjustLightness(hex: string, delta: number): string {
		const rgb = hexToRgb(hex);
		if (!rgb) return hex;
		const { h, s, l } = rgbToHsl(rgb.r, rgb.g, rgb.b);
		const l2 = clamp(l + delta, 0, 100);
		const { r, g, b } = hslToRgb(h, s, l2);
		return rgbToHex(r, g, b).toUpperCase();
	}

	export const colorPresets = [
		'#5B57E2',
		'#FF3B30',
		'#007AFF',
		'#FFD60A',
		'#34C759',
		'#000000',
		'#FFFFFF',
		'#FF9500'
	];
	export const AlbumColorsPresets = [
		'#ff595e',
		'#ffca3a',
		'#ff006e',
		'#8338ec',
		'#3a86ff',
		'#e07a5f',
		'#81b29a'
	];
}
