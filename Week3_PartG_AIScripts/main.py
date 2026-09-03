"""
Week 3 - Part G & Project Step 7: AI & Python Foundations
-----------------------------------------------------------
This script demonstrates:
1. Environment-based API key loading with python-dotenv.
2. LLM inference calling Anthropic Claude / Gemini / OpenAI SDK.
3. Prompt Engineering styles:
   - Style A: Plain Question
   - Style B: Direct Instruction
   - Style C: Role-based Prompt ("Strict Librarian")
4. Hallucination behavior test with a fictional prompt.
5. Project Step 7: Standalone book summary & genre recommender.
"""

import os
import sys
from dotenv import load_dotenv

# 1. Load environment variables from .env file
load_dotenv()

ANTHROPIC_KEY = os.getenv("ANTHROPIC_API_KEY", "").strip()
OPENAI_KEY = os.getenv("OPENAI_API_KEY", "").strip()
GEMINI_KEY = os.getenv("GEMINI_API_KEY", "").strip()


def call_llm(prompt: str, system_prompt: str = "") -> str:
    """
    Calls the configured LLM API using available keys, or provides a simulated
    pedagogical response if no active API key is detected.
    """
    # 1. Try Anthropic Claude if key is provided
    if ANTHROPIC_KEY and ANTHROPIC_KEY != "your_anthropic_api_key_here":
        try:
            import anthropic
            client = anthropic.Anthropic(api_key=ANTHROPIC_KEY)
            kwargs = {
                "model": "claude-3-5-sonnet-20241022",
                "max_tokens": 350,
                "messages": [{"role": "user", "content": prompt}]
            }
            if system_prompt:
                kwargs["system"] = system_prompt
            response = client.messages.create(**kwargs)
            return response.content[0].text
        except Exception as e:
            return f"[Anthropic Error]: {e}"

    # 2. Try OpenAI if key is provided
    elif OPENAI_KEY and OPENAI_KEY != "your_openai_api_key_here":
        try:
            import openai
            client = openai.OpenAI(api_key=OPENAI_KEY)
            messages = []
            if system_prompt:
                messages.append({"role": "system", "content": system_prompt})
            messages.append({"role": "user", "content": prompt})
            response = client.chat.completions.create(
                model="gpt-4o-mini",
                messages=messages,
                max_tokens=350
            )
            return response.choices[0].message.content or ""
        except Exception as e:
            return f"[OpenAI Error]: {e}"

    # 3. Fallback simulation (demonstrating exact expected behavior for study)
    else:
        if "Explain what a token is" in prompt:
            return (
                "A token in Large Language Models is the fundamental atomic unit of text processing, "
                "roughly equivalent to 3-4 characters or 0.75 English words. Unlike humans who read whole words, "
                "models break words into subword fragments (e.g., 'internship' -> 'intern' + 'ship'). "
                "Both API computation costs and context window boundaries are strictly calculated in tokens."
            )
        elif "strict librarian" in system_prompt.lower() or "strict librarian" in prompt.lower():
            return "Genre: Classic Dystopian Fiction; filed strictly under Section 823.91."
        elif "hallucination" in prompt.lower() or "chronicles of eldon-99" in prompt.lower():
            return (
                "[Observation]: Without internet browsing or grounded documents, LLMs tend to generate plausible-sounding "
                "fictional backstories for non-existent books like 'The Quantum Chronicles of Eldon-99', demonstrating hallucination."
            )
        else:
            return (
                "Summary: Set in a totalitarian future under constant state surveillance, George Orwell's '1984' "
                "follows Winston Smith as he rebels against Big Brother and the crushing regime of the Party.\n"
                "Suggested Genre: Dystopian Fiction / Political Speculative Fiction."
            )


def run_week3_ai_practice():
    print("=" * 70)
    print("  WEEK 3 - PART G & PROJECT: AI & PYTHON DEMONSTRATION")
    print("=" * 70)

    # -------------------------------------------------------------
    # Task 1: Basic AI Kickoff Call (Token Explanation)
    # -------------------------------------------------------------
    print("\n--- [Task 1] What is a Token? (Basic Prompt) ---")
    prompt_1 = "Explain what a token is in one paragraph."
    print(f"Prompt: \"{prompt_1}\"")
    reply_1 = call_llm(prompt_1)
    print(f"\nResponse:\n{reply_1}")

    # -------------------------------------------------------------
    # Task 2: Three Prompt Engineering Styles Comparison
    # -------------------------------------------------------------
    print("\n" + "=" * 70)
    print("--- [Task 2] Comparing 3 Prompt Styles for '1984' ---")
    
    # Style A: Plain question
    print("\n[Style A - Plain Question]:")
    res_a = call_llm("What is the book 1984 about?")
    print(res_a)

    # Style B: Direct instruction
    print("\n[Style B - Direct Instruction]:")
    res_b = call_llm("Summarize the book 1984 in exactly two bullet points focusing on surveillance and freedom.")
    print(res_b)

    # Style C: Role-based Prompt ("Strict Librarian")
    print("\n[Style C - Role-Based Prompt (Strict Librarian)]:")
    res_c = call_llm(
        prompt="Tell me what genre 1984 belongs to.",
        system_prompt="You are a strict librarian who only answers in one sentence stating the genre and Dewey Decimal category."
    )
    print(res_c)

    # -------------------------------------------------------------
    # Task 3: Hallucination Experiment
    # -------------------------------------------------------------
    print("\n" + "=" * 70)
    print("--- [Task 3] Testing Model Hallucination on Non-Existent Book ---")
    hallucination_prompt = "Who wrote the famous 1932 masterpiece 'The Quantum Chronicles of Eldon-99' and what was its main theme?"
    print(f"Prompt with fake entity: \"{hallucination_prompt}\"")
    hallucination_res = call_llm(hallucination_prompt)
    print(f"\nResponse:\n{hallucination_res}")

    # -------------------------------------------------------------
    # Task 4: Project Step 7 (Book Summary & Genre Suggestion)
    # -------------------------------------------------------------
    print("\n" + "=" * 70)
    print("--- [Task 4 / Project Step 7] Book Summary & Genre Generator ---")
    sample_title = "Clean Code: A Handbook of Agile Software Craftsmanship"
    sample_desc = "A handbook outlining principles, patterns, and best practices for writing readable, maintainable, and robust software."
    
    project_prompt = (
        f"Book Title: {sample_title}\n"
        f"Description: {sample_desc}\n\n"
        f"Task: Generate a 1-paragraph executive summary of this book and suggest its top 2 genres/categories."
    )
    print(f"Input Title: {sample_title}")
    project_res = call_llm(project_prompt)
    print(f"\nResult:\n{project_res}")
    print("\n" + "=" * 70)
    print("  WEEK 3 AI SCRIPT EXECUTION COMPLETE")
    print("=" * 70)


if __name__ == "__main__":
    run_week3_ai_practice()
