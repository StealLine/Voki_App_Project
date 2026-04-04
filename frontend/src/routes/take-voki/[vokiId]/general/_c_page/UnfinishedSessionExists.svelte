<script lang="ts">
	import GeneralVokiUnfinishedSessionView from '$lib/components/voki_types_specific/general/GeneralVokiUnfinishedSessionView.svelte';

	interface Props {
		vokiId: string;
		questionsWithSavedAnswersCount: number;
		sessionId: string;
		startedAt: Date;
		totalQuestionsCount: number;
	}
	let { questionsWithSavedAnswersCount, sessionId, startedAt, totalQuestionsCount, vokiId }: Props =
		$props();

	let takeVokiPageLink = $derived(`/take-voki/${vokiId}/general`);
</script>

<div class="unfinished-session-exists">
	<GeneralVokiUnfinishedSessionView
		sessionData={{
			vokiId,
			sessionId,
			startedAt,
			questionsWithSavedAnswersCount,
			totalQuestionsCount
		}}
		{takeVokiPageLink}
		replaceStateOnGoto={true}
	/>
</div>

<style>
	.unfinished-session-exists {
		display: flex;
		justify-content: center;
		margin: 2rem auto;
	}
</style>
