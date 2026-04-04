<script lang="ts">
	import type { GeneralVokiTypeSpecificData } from '../../../../types';
	import TypeSpecificDataBooleanField from './_c_shared/TypeSpecificDataBooleanField.svelte';

	interface Props {
		data: GeneralVokiTypeSpecificData;
	}
	let { data }: Props = $props();
</script>

<TypeSpecificDataBooleanField
	fieldName="Any audio content:"
	value={data.anyAudios}
	trueLabel="This Voki has audio content"
	falseLabel="No audio content"
/>
<TypeSpecificDataBooleanField
	fieldName="Answering order:"
	value={data.forceSequentialAnswering}
	trueLabel="Sequential"
	falseLabel="Free"
/>
<TypeSpecificDataBooleanField
	fieldName="Shuffle questions:"
	value={data.shuffleQuestions}
	trueLabel="Shuffled"
	falseLabel="Not shuffled"
/>
