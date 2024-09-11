<template>
    <div class="shadow bg-zinc-800 px-3 py-5 rounded-xl max-w-[500px]">
        <h1 class="text-4xl font-medium hover:text-purple-500 transition">Login Form</h1>

        <form class="mt-3">
            <input type="email" v-model="form.email" class="bg-zinc-900 mt-2 w-full p-2 rounded-lg"
                placeholder="Enter your email" />
            <input type="password" v-model="form.password" class="bg-zinc-900 mt-2 w-full p-2 rounded-lg"
                placeholder="Enter your password" />
        </form>

        <!-- display all errors above the buttons -->
        <div v-if="errors.length" class="mt-4 mb-2 text-sm p-2 bg-red-800 text-red-300 rounded-lg text-left">
            <ul>
                <li v-for="(message, index) in errors" :key="index">
                    {{ message }}
                </li>
            </ul>
        </div>

        <div class="flex space-x-1">
            <router-link to="/" class="text-lg mt-3">
                <custom-button variant="secondary">
                    Go Back
                </custom-button>
            </router-link>
            <custom-button class="mt-3 w-full" @click="submit">
                Login
            </custom-button>
        </div>
    </div>
</template>
<script setup lang="ts">
import { reactive, ref } from 'vue';
import { makeHttpRequest } from '../../core/http';
import { isValidationError, isErrorMessage } from '../../core/types/api-responses';
import CustomButton from '../CustomButton.vue';

// form data 
const form = reactive({
    email: "",
    password: "",
});

// error state
const errors = ref<string[]>([]);

// submit function
const submit = async () => {
    errors.value = [];

    const response = await makeHttpRequest("http://localhost:5196/v1/api/auth/login", "POST", form, {
        "Content-Type": "application/json"
    });

    if (isValidationError(response)) {
        for (const fieldErrors of Object.values(response.errors)) {
            errors.value.push(...fieldErrors);
        }
    }
    else if (isErrorMessage(response)) {
        if (response.detail) {
            errors.value.push(response.detail);
        }
    }
    else {
        console.log("Successful Login...")

        // get the jwt and save it to the local storage
        localStorage.setItem("jwt_token", response.data.token)
    }
};
</script>
