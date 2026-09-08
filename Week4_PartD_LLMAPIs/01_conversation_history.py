"""
Week 4 - Part D: Multi-turn Conversation History
Demonstrates maintaining state across stateless LLM API calls.
"""

import os
from dotenv import load_dotenv

load_dotenv()

def run_conversation_demo():
    print("=== Multi-turn Conversation State Demo ===")
    
    # Simulating conversation messages array
    history = [
        {"role": "user", "content": "Hello! My favorite book is 'The Pragmatic Programmer' and I love Clean Code."}
    ]
    
    print("User: Hello! My favorite book is 'The Pragmatic Programmer' and I love Clean Code.")
    print("Assistant: [Model generates reply acknowledging your favorite books]\n")
    
    history.append({
        "role": "assistant",
        "content": "Nice to meet you! 'The Pragmatic Programmer' and 'Clean Code' are fantastic software engineering classics."
    })
    
    # Second turn - the model needs the entire history array resent
    history.append({
        "role": "user",
        "content": "Can you recommend two more books similar to my favorite ones?"
    })
    
    print("User: Can you recommend two more books similar to my favorite ones?")
    print("--> Entire message history is sent to API to maintain contextual memory.")
    print(f"Total history payload items: {len(history)}")

if __name__ == "__main__":
    run_conversation_demo()
