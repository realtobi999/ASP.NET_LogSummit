<template>
    <button :class="buttonClass" :disabled="isDisabled" @click="onClick">
        <slot></slot>
    </button>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps({
    type: {
        type: String,
        default: 'button',
    },
    disabled: {
        type: Boolean,
        default: false,
    },
    variant: {
        type: String,
        default: 'primary', // Can be 'primary', 'secondary', etc.
    },
    size: {
        type: String,
        default: 'md', // Can be 'sm', 'md', 'lg'
    },
});

const emit = defineEmits(['click']);

const isDisabled = computed(() => props.disabled);
const buttonClass = computed(() => {
    const sizeClasses = {
        sm: 'min-w-[100px] px-3 py-2 text-sm',  
        md: 'min-w-[120px] px-4 py-2 text-base', 
        lg: 'min-w-[150px] px-6 py-3 text-lg',   
    }[props.size];

    const variantClasses = {
        primary: 'bg-purple-500 text-white font-medium hover:bg-purple-600',
        secondary: 'bg-zinc-600 text-white font-medium hover:bg-zinc-700',
        danger: 'bg-red-500 text-white hover:bg-red-600',
    }[props.variant];

    const disabledClasses = props.disabled ? 'opacity-50 cursor-not-allowed' : '';

    return `inline-flex items-center justify-center whitespace-nowrap transition ease-in-out ${sizeClasses} ${variantClasses} ${disabledClasses}`;
});


function onClick(event: MouseEvent) {
    if (!isDisabled.value) {
        emit('click', event);
    }
}
</script>