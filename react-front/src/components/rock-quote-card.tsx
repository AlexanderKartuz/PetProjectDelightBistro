import React from 'react';

interface RockQuoteProps 
{
    name: string;
    url: string;
    quote_text: string;
}

export const RockQuoteCard: React.FC<RockQuoteProps> = ({ name, url, quote_text }) => {
    return (
        <div className="rockStar quote-card-template" style={{ display: 'block' }}>
            <div className="rockStar-header quote-card-layout">
                <div className="image-container quote-avatar-box">
                    <img src={url} alt={name} />
                </div>
                <div className="quote-card-body">
                    <div className="rockStar-name quote-author">{name}</div>
                    <div className="quote-text-content">"{quote_text}"</div>
                </div>
            </div>
        </div>
    );
};
