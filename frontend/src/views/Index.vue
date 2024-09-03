<template>
    <div class="flex flex-col items-center">
        <img src="./../../public/icon.png" alt="LogSummit Logo" class="w-[255] h-[255px] object-cover" />
        <div class="flex flex-col items-center">
            <h1 class="hover:text-purple-500 font-medium transition duration-250 ease-out-cubic text-6xl mt-6">
                LogSummit
            </h1>

            <div class="mt-5 flex space-x-1">
                <custom-button size="md">
                    <router-link to="/" class="text-lg">
                        Sign Up
                    </router-link>
                </custom-button>
                <custom-button size="md">
                    <router-link to="/" class="text-lg">
                        Login
                    </router-link>
                </custom-button>
            </div>
        </div>

    </div>
</template>

<script setup>
import CustomButton from '../components/CustomButton.vue';
</script>