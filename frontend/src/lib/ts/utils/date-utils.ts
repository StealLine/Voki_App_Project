import { browser } from "$app/environment";

export namespace DateUtils {
	export function toLocale(date: Date): string {
		const locale = browser ? navigator.language : 'en-US';

		const timePart = date.toLocaleTimeString(locale, {
			hour: '2-digit',
			minute: '2-digit',
			hour12: false,
		});

		return `${timePart} ${toLocaleDateOnly(date)}`;
	}

	export function toLocaleDateOnly(date: Date): string {
		const locale = browser ? navigator.language : 'en-US';

		return date.toLocaleDateString(locale, {
			day: '2-digit',
			month: '2-digit',
			year: 'numeric',
		});
	}

	export function formatDuration(start: Date, end: Date): string {
		const totalSeconds = Math.floor(
			(end.getTime() - start.getTime()) / 1000
		);

		const hours = Math.floor(totalSeconds / 3600);
		const minutes = Math.floor((totalSeconds % 3600) / 60);
		const seconds = totalSeconds % 60;

		const parts: string[] = [];

		if (hours > 0) parts.push(`${hours}h`);
		if (minutes > 0 || hours > 0) parts.push(`${minutes}m`);
		parts.push(`${seconds}s`);

		return parts.join(' ');
	}

	export const isoDateRegex: RegExp =
		/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:Z|[+-]\d{2}:\d{2})?$/;


	export function startOfDay(d: Date): Date {
		return new Date(d.getFullYear(), d.getMonth(), d.getDate());
	}

	export function addDays(d: Date, days: number): Date {
		const nd = new Date(d);
		nd.setDate(nd.getDate() + days);
		return nd;
	}

	export function sameDay(a: Date, b: Date): boolean {
		return (
			a.getFullYear() === b.getFullYear() &&
			a.getMonth() === b.getMonth() &&
			a.getDate() === b.getDate()
		);
	}
}
