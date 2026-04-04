<script lang="ts">
	interface Props {
		date: Date | null;
		error: string | null;

		quickPickLabel: string;
		onQuickPick: () => void;

		onCustomDateSelect: () => void;
		onDateChange: (date: Date | null) => void;
	}

	let { date, error, quickPickLabel, onQuickPick, onCustomDateSelect, onDateChange }: Props =
		$props();

	const dateFormatterForNativeInput = (d: Date | null) => {
		if (!d) return '';
		const offset = d.getTimezoneOffset();
		const localDate = new Date(d.getTime() - offset * 60 * 1000);
		return localDate.toISOString().split('T')[0];
	};

	const handleDateChange = (e: Event) => {
		const input = e.target as HTMLInputElement;
		if (!input.value) {
			onDateChange(null);
			return;
		}
		onDateChange(new Date(input.value));
	};
</script>

<div class="filter-group">
	<div class="input-container">
		<div class="pill-toggle" role="group" aria-label="Date range">
			<button class="pill-btn" class:active={date === null} onclick={onQuickPick}>
				{quickPickLabel}
			</button>
			<button class="pill-btn" class:active={date !== null} onclick={onCustomDateSelect}>
				Custom date
			</button>
		</div>
		{#if date !== null}
			<input
				type="date"
				value={dateFormatterForNativeInput(date)}
				onchange={handleDateChange}
				class="custom-date-input"
				class:error
				tabindex={date === null ? -1 : 0}
			/>
		{/if}
		{#if error}
			<span class="error-msg">{error}</span>
		{/if}
	</div>
</div>

<style>
	.filter-group {
		display: flex;
		flex-direction: row;
		align-items: center;
		gap: 0.75rem;
	}

	.input-container {
		display: flex;
		flex-direction: row;
		gap: 0.5rem;
		position: relative;
		align-items: center;
	}

	.pill-toggle {
		display: flex;
		flex-direction: row;
		border-radius: 999px;
		box-shadow: var(--shadow-xs);
		overflow: hidden;
		gap: 0.25rem;
		padding: 0.25rem;
	}

	.pill-btn {
		padding: 0.5rem 1rem;
		border: none;
		border-radius: 999px;
		background: transparent;
		color: var(--muted-foreground);
		font-family: inherit;
		font-size: 1rem;
		font-weight: 500;
		cursor: pointer;
		transition:
			background-color 0.18s ease,
			color 0.18s ease,
			box-shadow 0.18s ease;
		white-space: nowrap;
		outline: none;
	}

	.pill-btn:hover:not(.active) {
		background-color: var(--muted);
		color: var(--text);
	}

	.pill-btn.active {
		background: var(--accent);
		color: var(--primary);
	}

	.custom-date-input {
		box-sizing: border-box;
		padding: 0.5rem 0.75rem;
		border-radius: 999px;
		border: 0.125rem solid var(--muted);
		background-color: var(--back);
		color: var(--text);
		font-family: inherit;
		font-size: 0.875rem;
		font-weight: 500;
		outline: none;
		transition:
			border-color 0.18s,
			box-shadow 0.18s,
			background-color 0.18s,
			opacity 0.15s;
		cursor: pointer;
	}

	.custom-date-input.error {
		background-color: var(--red-1);
		color: var(--red-3);
		border-color: var(--red-3);
	}

	.error-msg {
		position: absolute;
		top: calc(100% + 0.5rem);
		left: 50%;
		transform: translateX(-50%);
		font-size: 0.875rem;
		color: var(--red-3);
		background-color: var(--red-2);
		white-space: nowrap;
		font-weight: 500;
		padding: 0.125rem 0.75rem;
		border-radius: 999px;
	}
</style>
