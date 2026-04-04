<script lang="ts">
	import type { ResponseResult } from '$lib/ts/backend-communication/result-types';
	import { AuthStore } from '$lib/ts/stores/auth-store.svelte';
	import { getSignInDialogOpenFunction } from '../../../../../_c_layout/_ts_layout_contexts/sign-in-dialog-context';
	import type { VokiRatingData } from '../../../types';
	import RatingsListItem from './_c_all_ratings/RatingsListItem.svelte';
	import RatingsTabUserRating from './_c_all_ratings/RatingsTabUserRating.svelte';
	import UserRatingCannotRateMessage from './_c_all_ratings/UserRatingCannotRateMessage.svelte';

	interface Props {
		allRatings: VokiRatingData[];
		hasUserTakenVoki: boolean;
		saveNewRating: (value: number) => Promise<ResponseResult<VokiRatingData>>;
	}
	let { allRatings, hasUserTakenVoki, saveNewRating }: Props = $props();

	type UserRatingStateType =
		| { name: 'not-taken' }
		| { name: 'unauthenticated' }
		| ({ name: 'rated' } & VokiRatingData)
		| { name: 'not-rated'; userId: string };

	let ratingsViewState: {
		userRatingState: UserRatingStateType;
		otherUserRatings: VokiRatingData[];
	} = $state(processedRatings());

	function processedRatings(): {
		userRatingState: UserRatingStateType;
		otherUserRatings: VokiRatingData[];
	} {
		let authStoreData = AuthStore.Get();

		if (authStoreData.isAuthenticated && hasUserTakenVoki) {
			const userRatingValue = allRatings.find((r) => r.userId === authStoreData.userId);
			if (userRatingValue) {
				return {
					userRatingState: {
						name: 'rated',
						...userRatingValue
					},
					otherUserRatings: allRatings.filter((r) => r.userId !== authStoreData.userId)
				};
			}
			return {
				userRatingState: {
					name: 'not-rated',
					userId: authStoreData.userId!
				},
				otherUserRatings: allRatings
			};
		} else if (authStoreData.isAuthenticated && !hasUserTakenVoki) {
			{
				return {
					userRatingState: { name: 'not-taken' },
					otherUserRatings: allRatings
				};
			}
		} else {
			return {
				userRatingState: { name: 'unauthenticated' },
				otherUserRatings: allRatings
			};
		}
	}
	const openSignInDialog = getSignInDialogOpenFunction();
</script>

	{#if ratingsViewState.userRatingState.name === 'unauthenticated'}
		<UserRatingCannotRateMessage mainText="To rate Vokis you need to be logged in">
			<div class="auth-needed-to-rate-btns">
				<button class="login-btn" onclick={() => openSignInDialog('login')}>Login</button>
				<button class="signup-btn" onclick={() => openSignInDialog('signup')}
					>I don't have an account yet</button
				>
			</div>
		</UserRatingCannotRateMessage>
	{:else if ratingsViewState.userRatingState.name === 'not-taken'}
		<UserRatingCannotRateMessage mainText="To rate this Voki you need to take it first" />
	{:else}
		<RatingsTabUserRating ratingState={ratingsViewState.userRatingState} {saveNewRating} />
	{/if}
	{#if ratingsViewState.userRatingState.name === 'rated' && ratingsViewState.otherUserRatings.length === 0}
		<div class="no-other-ratings">No one else has rated this Voki</div>
	{:else if ratingsViewState.otherUserRatings.length != 0}
		{#each ratingsViewState.otherUserRatings as rating}
			<RatingsListItem
				userId={rating.userId}
				content={{ name: 'default', ratingValue: rating.value }}
				dateTime={rating.dateTime}
			/>
		{/each}
	{/if}

	<style>
		.auth-needed-to-rate-btns {
			display: flex;
			justify-items: center;
			gap: 2rem;
		}

		.auth-needed-to-rate-btns > button {
			padding: 0.25rem 1rem;
			border: none;
			border-radius: 0.25rem;
			background-color: var(--primary);
			color: var(--primary-foreground);
			font-size: 1.125rem;
			font-weight: 420;
			cursor: pointer;
		}

		.auth-needed-to-rate-btns > button:hover {
			background-color: var(--primary-hov);
		}

		.no-other-ratings {
			margin: 0 auto;
			color: var(--secondary-foreground);
			font-size: 1.125rem;
			font-weight: 450;
		}
	</style>
