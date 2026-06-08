import React, { useState, useEffect } from 'react';
import { RockQuoteCard } from './rock-quote-card';

interface QuoteData {
    id?: number;
    name: string;
    url: string;
    quote_text: string;
}

export const RockQuotesContainer: React.FC = () => {
    const [quotes, setQuotes] = useState<QuoteData[]>([]);

    useEffect(() => {
        fetch('https://localhost:7042/GetQuotes') 
            .then(response => response.json())
            .then(data => 
            {
                console.log("data api", data);
                setQuotes(data);
            })
            .catch(error => console.error("Ошибка загрузки рок-цитат:", error));
    }, []);

    return (
        <div className="main-container quote-page-container">
            <h1 className="quote-page-title">Мудрость Рок-Легенд (React)</h1>
            <div className="quotes-catalog-grid">
                {quotes.map((quote, index) => (
                    <RockQuoteCard 
                        key={quote.id || index}
                        name={quote.name}
                        url={quote.url}
                        quote_text={quote.quote_text}
                    />
                ))}
            </div>
        </div>
    );
};
