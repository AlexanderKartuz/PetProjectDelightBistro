
$(document).ready(function () 
{
    $('li.spire-hero-card').click(function(event)
    {
        if ($(event.target).closest('.spire-hero-card-edit').length > 0)
        {
            return;
        }

        const self = $(this);
        
        self.toggleClass('active');
        updateDeleteButtonState()
    });   
    
    $('.spire-hero-cards-deleteButton .delete-card').click(function()
    {
        $('li.spire-hero-card.active').remove();
        updateDeleteButtonState()
    });

    function updateDeleteButtonState()
    {
        const deleteBtn = $('.spire-hero-cards-deleteButton .delete-card');
        const hasActiveCard = $('li.spire-hero-card.active').length > 0;

        if (hasActiveCard) 
        {
            deleteBtn.removeAttr('disabled');
        } 
        else 
        {
            deleteBtn.attr('disabled', 'disabled');
        }
    } 
});


